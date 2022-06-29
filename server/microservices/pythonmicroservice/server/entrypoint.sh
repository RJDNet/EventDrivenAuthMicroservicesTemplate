#!/bin/bash

set -e

sleep 30
run_cmd="python server.py"

exec $run_cmd
