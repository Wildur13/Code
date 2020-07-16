module NatsType
open Mini

type Nats = | Nil | Cons of Nat * Nats
