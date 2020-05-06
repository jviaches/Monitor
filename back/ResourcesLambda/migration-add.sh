#!/bin/bash
dotnet ef --startup-project ResourcesLambda migrations add $1 --project Monitor.Infra