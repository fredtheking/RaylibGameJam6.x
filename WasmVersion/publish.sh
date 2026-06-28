#!/bin/bash
rm -rf obj bin
echo CLEANED OBJ AND BIN
dotnet restore 
dotnet publish -c Release
