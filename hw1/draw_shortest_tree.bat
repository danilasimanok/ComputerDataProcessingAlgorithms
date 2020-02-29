@echo off
set pathfinder=Tasks/"Pathfinder(joke)"/bin/Debug/netcoreapp3.1/"Pathfinder(joke).exe"
call %pathfinder% %1 %2.temp
call dot -Tpdf -o%2 %2.temp
del %2.temp