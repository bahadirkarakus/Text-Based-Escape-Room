import matplotlib.pyplot as plt
import numpy as np

# Define the map layout
map_layout = [
    "##########",
    "#@   #  P#",
    "#   ##   #",
    "# *      #",
    "#   ##   #",
    "#      E #",
    "##########"
]

# Create a figure and axis
fig, ax = plt.subplots(figsize=(6, 6))

# Define colors for the map elements
element_colors = {
    '#': 'black',  # Walls
    '@': 'blue',   # Player
    'P': 'green',  # Portal
    '*': 'yellow', # Item
    'E': 'red',    # Enemy
    ' ': 'white'   # Empty space
}

# Draw the map
for y, row in enumerate(map_layout):
    for x, char in enumerate(row):
        color = element_colors.get(char, 'white')
        rect = plt.Rectangle((x, -y), 1, 1, color=color)
        ax.add_patch(rect)

# Set axis limits and remove ticks
ax.set_xlim(0, len(map_layout[0]))
ax.set_ylim(-len(map_layout), 0)
ax.set_aspect('equal')
ax.axis('off')

# Save the figure
plt.savefig("c:/Users/Casper/Desktop/ProjePL/ConsoleApp1/map_visual.png")
plt.close()