module RingBuffer

open Mini
open RingBufferTypes

// a)
let create<'T> (capacity: Int): RingBuffer<'T> =
    {
        buffer = Array.zeroCreate capacity;
        size = ref 0 ;
        writePos = ref 0    
    }

// b)
let get<'T> (r: RingBuffer<'T>): 'T option =
    match !r.size with
    |0-> None 
    |_-> let b = r.buffer.[(((!r.writePos) - (!r.size))+r.buffer.Length)% (r.buffer.Length)]
         r.size := !r.size - 1
         Some b                 

// c)
let put<'T> (r: RingBuffer<'T>) (elem: 'T): unit =
    r.buffer.[!r.writePos] <- elem 
    if (!r.writePos + 1)< r.buffer.Length then              
            r.writePos := !r.writePos + 1
    else 
            r.writePos := 0   
    if !r.size < r.buffer.Length then 
        r.size := !r.size + 1             
    (*r.buffer.[!r.writePos] <- elem 
    if !r.writePos+1 = r.buffer.Length then 
        r.writePos := 0
    else 
        r.writePos := !r.writePos + 1
    if !r.size < r.buffer.Length then 
        r.size := !r.size + 1                     
    *)
