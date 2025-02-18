# Custom Gravity for Exiled
Adds highly customizable gravity modifications to the server.

Introduces both very customizable gravity system for **Items/Pickus** and **Players**.

## Preview Video
[![Preview Video](https://img.youtube.com/vi/e3DH_n_fewk/0.jpg)](https://www.youtube.com/watch?v=e3DH_n_fewk)

# Commands
The command is very easy to use with the advanced gravity system I created.

### Set the gravity for a specific player:
- `setgravity (Player Name / Player ID) (x) (Y) (Z)`

### Set a new default gravity for everyone on the server (takes effect immediately):
- `setgravity (x) (Y) (Z)`

### Reset gravity for everyone (resets to the gravity set in the config):
- `setgravity reset`
  
### ‎ 
### ‎ 

### Set the gravity for every Pickup in the server:
- `pickupgravity (X) (Y) (Z)`

### Set the gravity for spesific Item Type in the server:
- `pickupgravity (ItemType) (X) (Y) (Z)`
- Here you can find the Item Types that you can use.

# Config
You can control the default gravity directly from the config.
You can change default gravity for **Players** and also you can change gravity value for **every spesific Item Type**:
```yaml
# A Gravity Vector. The default value is (-19.6). This will be the default Gravity value of the server.
gravity_vector:
- 0
- -3
- 0

# A Gravity Vector for pickups. The default value is (-19.6). This will be the default Gravity value of the Pickups in the server.
pickup_gravity_vector:
- 0
- -1
- 0

# A gravity vector for each item type, check my github page for item names. These pickups will have their own gravity value. Not effected by default pickup gravity vector.
spesific_item_gravity:
  Medkit:
  - 0
  - -19.6
  - 0
  MicroHID:
  - 0
  - -19.6
  - 0
```

And uhh yeah.. that is all. Its basically making you able to alter with the gravity however you want...
Thats it...
