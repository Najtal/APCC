# APCC
Adaptative Process Cpu Consumption

This project is entended to be added to realtime systems. It's purpose is to set a server that can registr client (A client is a process).
When a client connects, it share it's PID, if he is a realTime process, a priority order and a flexibility.
The server checks all clients cpu charge and when the cpu charge becomes to heavy, it will ask to low priority processes to adapt their behaviour to save CPU power. 

For example, a autonomous drone wich shares video can dynamically change it's video quality to make sure the autopilot process can run safely.
