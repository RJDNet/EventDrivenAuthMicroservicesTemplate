#!/bin/bash

set -e

sleep 30
run_cmd="dotnet csharpmicroservice.dll"

exec $run_cmd
