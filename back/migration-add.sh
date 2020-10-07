#!/bin/bash
dotnet ef --startup-project monitor-back migrations add $1 --project monitor-infra