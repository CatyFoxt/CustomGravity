# Custom Gravity for Lab API
Adds highly customizable gravity modifications to the server.

# Commands
The command is very easy to use with the advanced gravity system I created.

### Set the gravity for a specific player:
- `setgravity (Player Name / Player ID) (x) (Y) (Z)`

### Set a new default gravity for everyone on the server (takes effect immediately):
- `setgravity (x) (Y) (Z)`

### Reset gravity for everyone (resets to the gravity set in the config):
- `setgravity reset`

### Aliases:
- `setgravity`
- `setgrav`
- `setg`

# Config
You can control the default gravity directly from the config:
```yaml
# Controls front/back gravity.
gravity_vector_x: 0
# Controls up/down gravity.
gravity_vector_y: -5
# Controls right/left gravity.
gravity_vector_z: 0
```

And uhh yeah.. that is all. Its basically making you able to alter with the gravity however you want...
Thats it...
