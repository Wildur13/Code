module RingBufferTypes

open Mini

type RingBuffer<'T> =
    { buffer: Array<'T>
      size: Int ref
      writePos: Int ref}
