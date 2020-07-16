module Queues 

open Mini
open QueuesTypes


// Der auf dem Übungsblatt abgebildete Baum zum Testen
let ex = Node (Node(Node(Empty,1N,Empty),2N,Node(Empty,3N,Empty)) , 4N, Node(Node(Empty,5N,Empty),6N,Node(Empty,7N,Empty)))


// a)
let simpleQueue<'T> (): IQueue<'T> =
    let mutable list = []
    {
        new IQueue<_> with 
            member self.Add (a:'T) = 
                list <- list @ [a]
            member self.Remove () = 
                match list with 
                |[]    -> None
                |x::xs -> 
                          list <- xs
                          Some x

    }

// b)
let advancedQueue<'T> (): IQueue<'T> = 
    let mutable rear = []
    let mutable front = []
    {
        new IQueue<_> with 
            member self.Add (a:'T) = 
                front <-  a :: front 
            member self.Remove (): 'T option = 
                match rear with 
                |[] -> match front with 
                        |[] -> None 
                        |a::b -> 
                            rear <- List.rev (front)
                            front <- []
                            self.Remove() 
                |x::xs -> 
                      rear <- xs 
                      Some x                                                                 

    }

// c)
let rec enqueue (q: IQueue<'T>) (elems: 'T list): Unit =
    match elems with 
    |[] -> ()
    |x::xs -> q.Add x; enqueue q xs

let rec dequeue (q: IQueue<'T>): 'T list =
    match q.Remove() with
    |None -> []
    |Some a -> a :: dequeue q 


// d)
let rec bft (q: IQueue<Tree<'T>>): 'T list  =
    match q.Remove() with 
    |None -> []
    |Some a -> match a with 
                | Empty -> bft q
                | Node (l,w,r) -> q.Add l; q.Add r; w :: bft q


let rec missing (l:Nat list): Nat =
    let rec desc (i:Nat, list1:Nat list): Nat = 
       if List.contains(i) (list1) then 
          desc(i+1N, list1)
        else i 
    in desc(0N, l)

let Kth (l:Nat list) (n:Nat): bool = 
   List.contains n l 


let vorwahl (n:Nat): bool= 
         let a = string n 
         in a.[0] = "0" && a.[1] = "6" && a.[2]="3" && a.[3]= "1"


let iota (n:Nat) : Nat list = 
    [for i in 0N .. (n- 1N) do yield i]

let rec Check (n:Nat) : bool = 
    let a = n+1N
    if n= 0N then true 
    elif a = 2N || (a%2N) = 0N then (if a = 2N then true else Check (n/2N))
    else false 

let rec contains (a:Nat) (l: Nat list) : bool = 
    match l with 
    |[]-> false
    | x::xs -> a = x || contains a xs 

let rec distinct (l:Nat list): Nat list = 
    match l with 
    |[]-> []
    |a:: b -> if contains a b then 
                            let res = distinct b 
                            List.sort res
              else a:: distinct b

let vorwahl (n:Nat ) : bool = 
    let a = string n 
    a.[0]= '0' && a.[1]= '6' && a.[2]= '3' && a.[3]= '1' 

let Odervorwahl (n:Nat): bool = 
    let rec hilfe (l:Nat): Nat list =
        if l = 0N then [0N]
                  else (l%10N :: hilfe (l/10N)) 
    let a (m:Nat list) (s:Nat) : bool =
        let pos = string s
        if pos.Length < m.Length then 
             let chg = m.Length - 1
              in m.[chg]= 0N && m.[chg - 1]= 6N && m.[chg- 2]= 3N && m.[chg- 3]= 1N 
        else  m.[m.Length]= 0N && m.[m.Length- 1]= 6N && m.[m.Length- 2]= 3N && m.[m.Length- 3]= 1N     
    in a (hilfe (n)) n              
   
let rec rev (n: Nat list):Nat list =
    match n with
    |[] -> []
    |x::xs -> (rev xs) @ [x]

let rec hilfe (l:Nat list) (p:Nat list) : Nat list = 
        match l,p with 
        |([],_)|(_,[]) -> []
        |a::b,c::d -> (a*c):: hilfe b d

let rec Mult (n:Nat list list ) (m:Nat list list): Nat list list = 
    match n with 
     |[[]]-> [[]]
     |(a::b)::xs -> match m with 
                    |[[]]->[[]]
                    |(c::d)::ys ->   ((a*c) :: hilfe b d) :: Mult xs ys 