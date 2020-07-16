module SearchTypes

type PTree<'T> = Tree<'T> ref
and Tree<'T> = 
| Empty 
| Node of PTree<'T> * 'T * PTree<'T>
