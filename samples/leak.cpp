#include <conio.h>
#include <ctype.h>
#include <stdio.h>
#include <malloc.h>
#include <windows.h>

/*
GlowCode program demonstrating memory leak detection

To build: start a command window for a target compiler, then run leak.build.bat.

Leak detection matrix for various C-runtime configurations

		debug-dll		rel-dll			debug-static	rel-static
		===================================================================
VS8		yes*			yes				yes					yes
VS7.1 	yes*			yes				no					no
VS6.0	yes*			yes				no					no

*the last block allocated can not be detected as a leak because the debug heap maintains an internal pointer to it.

*/


struct CBlock {
	CBlock* pPrev;
	CBlock()
		:pPrev(0)
	{
	}
};

bool g_bMalloc = false;
int blockSize = 10;
int N_action = 1;

CBlock* g_pLast = 0;
HANDLE hWinHeap = 0;

void AllocateBlock()
{
	if ( blockSize < sizeof(CBlock) )
		return;
	CBlock* pBlock;
	if ( hWinHeap )
	{
		pBlock = (CBlock*)HeapAlloc( hWinHeap, 0, blockSize );
	}
	else if ( g_bMalloc )
	{
		pBlock = (CBlock*)malloc(blockSize );
	}
	else
	{
		pBlock = (CBlock*)new char[ blockSize ];
	}
	if ( g_pLast )
		pBlock->pPrev = g_pLast;
	else
		pBlock->pPrev = 0;
	g_pLast = pBlock;
}

void AllocateBlockRecursive( int c )
{
	AllocateBlock();
	if ( c > 0 )
		AllocateBlockRecursive( c-1 );
}

bool ReleaseBlock()
{
	if ( g_pLast )
	{
		CBlock *pBlock = g_pLast;
		g_pLast = g_pLast->pPrev;
		if ( hWinHeap )
			HeapFree( hWinHeap, 0, pBlock );
		else if ( g_bMalloc )
			free( pBlock );
		else
			delete pBlock;
		return true;
	}
	else
		return false;

}

bool LeakBlock()
{
	if ( g_pLast )
	{
		CBlock *pBlock = g_pLast;
		g_pLast = g_pLast->pPrev;
		pBlock->pPrev = 0;
		return true;
	}
	else
		return false;
}

int filter(unsigned int code, struct _EXCEPTION_POINTERS *ep) {
	printf( "Exception: 0x%x\n", code );
    return EXCEPTION_EXECUTE_HANDLER;
}

void AccessException()
{
	char* pc = 0;
	char c = *pc;
}

void AccessExceptionTry()
{
	__try
	{
		AccessException();
	}
    __except( filter(GetExceptionCode(), GetExceptionInformation()) )
	{
    }
}



int main( void )
{
    int ch;
	printf( "GlowCode memory allocate/release/leak demonstration:\n" );
	printf( "Type:\n" );
	printf( "'a' to allocate N blocks of size S\n" );
	printf( "'Alt-A' to allocate N blocks of size S recursively\n" );
	printf( "'r' to release N blocks\n" );
	printf( "'l' to leak N blocks\n" );
	printf( "\n" );
	printf( "'+' to use increase N by 10\n" );
	printf( "'-' to use reduce N by 10\n" );
	printf( "'Alt+' to use grow S by 10\n" );
	printf( "'Alt-' to use reduce S by 10\n" );
	printf( "\n" );
	printf( "'m' to use malloc/free\n" );
	printf( "'n' to use new/delete\n" );
	printf( "'w' to use HeapAlloc/HeapFree\n" );
	printf( "\n" );
	printf( "'e' to cause access exception\n" );
	printf( "\n" );
	printf( "'x' to exit\n" );
	printf( "\n" );

   do
   {
      ch = tolower(_getch());
	  SHORT keyState = GetAsyncKeyState( VK_MENU );
	  bool bAltKey = (keyState & 0x8000);
      switch ( ch )
      {
		case 'a':
			if ( bAltKey )
				AllocateBlockRecursive( N_action );
			else
			{
				for ( int i=0; i<N_action; i++ )
					AllocateBlock();
			}
			break;
		case 'r':
		{
			for ( int i=0; i<N_action; i++ )
				if ( !ReleaseBlock() )
				{
					ch = '\a'; //output a bell
					break;
				}
			break;
		}
		case 'l':
		{
			for ( int i=0; i<N_action; i++ )
				if ( !LeakBlock() )
				{
					ch = '\a'; //output a bell
					break;
				}
			break;
		}
		case 'm':
			g_bMalloc = true;
			g_pLast = 0;
			if ( hWinHeap )
				HeapDestroy( hWinHeap );
			hWinHeap = 0;
			printf( "\nPrevious allocations will be leaked\n" );
			break;
		case 'n':
			g_bMalloc = false;
			g_pLast = 0;
			if ( hWinHeap )
				HeapDestroy( hWinHeap );
			hWinHeap = 0;
			printf( "\nPrevious allocations will be leaked\n" );
			break;
		case 'w':
			if ( hWinHeap )
				HeapDestroy( hWinHeap );
			hWinHeap = HeapCreate( 0, 0, 0 );
			g_pLast = 0;
			printf( "\nPrevious allocations will be leaked\n" );
			break;
		case '+':
			if ( bAltKey )
			{
				if ( blockSize == 0 )
					blockSize = 10;
				else if ( blockSize < 1000000000 )
					blockSize = blockSize*10;
				printf( "+\nS = %i bytes/block\n", blockSize );
			}
			else
			{
				if ( N_action  < 10000000 )
					N_action *= 10;
				printf( "+\nN = %i blocks\n", N_action );
			}
			continue;
		case '-':
			if ( bAltKey )
			{
				if ( blockSize > 10 )
					blockSize /= 10;
				else
					blockSize = 0;
				printf( "-\nS = %i bytes/block\n", blockSize );
			}
			else
			{
				if ( N_action > 1 )
					N_action /= 10;
				printf( "+\nN = %i blocks\n", N_action );
			}
			continue;
		case 'e':
			AccessExceptionTry();
			continue;
		case 'x':
      		return 0;
		default: continue;
	}
	if ( ch )
	  printf( "%c", ch );
  } while( 1 );

}