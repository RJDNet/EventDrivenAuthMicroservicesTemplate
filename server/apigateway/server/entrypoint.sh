#!/bin/bash

set -e

sleep 30
run_cmd="dotnet apigateway.dll"

exec $run_cmd
