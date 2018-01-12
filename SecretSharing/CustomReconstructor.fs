﻿namespace SecretSharing

open System
open System.Collections.Generic
open System.Linq
open System.Numerics

type ICustomReconstructor<'a, 'b> =
    abstract member ReconstructSecret : Prime * List<'b> -> 'a

//Wraps the SecretReconstructor so you can deal with anything, rather than with bigints.
module CustomReconstructor =

    let getPassword
        (fromBigInt : bigint -> 'a)
        (toCoordinate : 'b -> Coordinate)
        (prime : bigint) (encodedCoords : 'b list) : 'a =

        encodedCoords
        |> List.map toCoordinate
        |> SecretReconstructor.getSecret prime
        |> fromBigInt

    let make (fromBigInt : Func<bigint, 'a>, toCoord : Func<'b, Coordinate>) =
        { new ICustomReconstructor<_, _> with
                member __.ReconstructSecret (prime, coords) : 'a =
                    let f = Function.fromFunc fromBigInt
                    let g = Function.fromFunc toCoord

                    coords
                    |> List.ofSeq
                    |> getPassword f g prime }