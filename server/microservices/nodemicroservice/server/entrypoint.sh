#!/bin/bash

set -e

sleep 30
run_cmd="node server.ts"

exec $run_cmd
