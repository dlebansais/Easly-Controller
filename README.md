# Easly-Controller

Manage and edit Easly source code.

Easly source code is defined in the [Easly-Language](https://github.com/dlebansais/Easly-Language/) assembly. A controller, created and initialized with any of the Easly objects, can display the source code content in a .NET WPF application, allowing modification of the source code.

# TODO

- [ ] Views.
- [ ] More layers.

# Architecture

The controller is made of several layers, each layer adding features to the other layer it's built on.

## Read-Only

This layer only provides read-only access to the source code, but introduces the core classes of the controller:

1. Indexes
An index is similar to a C++ iterator, but is fixed. It represents a child in a parent. There are five type of indexes:
+ Placeholder, a child node. This is the most straightforward index. This index has two specialized forms, one for block list replication patterns and another for block list source identifiers. 
+ Optional, a child node that is optionally assigned (throught the IOptionalReference<> interface).
+ List, a list of child nodes of the same type.
+ BlockList, a list of blocks of child nodes, all of the same type, with replication support (see the IBlockList<> interface). This index comes in two forms: an index for the first item in a block, and an index for all subsequent items in that block. Within the BlockList index, there are two variants:
  * One for a new block. This specifies the first item in the node list of the block
  * One for an existing block, to specify further items. 

Note that the read-only layer provides indexes that only allow reading source code. The next layer (Writeable, see below) introduces insertion indexes, describing children not yet inserted.

2. States
These classes represent all data associated to a particular node in the source code tree. For the read-only layer, there isn't much, but subsequent layers add more info, like the position in the screen and other temporary data. Blocks in block list also have a specific state class. And finally there are two dedicated state classes for block list replication patterns and another for block list source identifiers. 

3. Inners
They are the glue between states and indexes. There are as many types of inners as there are types of indexes, and given a state and inner one can obtain an index to the corresponding child.

4. Browse Context
Contexts (for short) contains data accumulated when the source code tree is parsed. For the read-only layer, this is simply a collection of inners for each state, but subsequent layers contains more.

5. Views
One or more views can be attached to a controller. For the read-only layer, they don't do much, but when clients of the controller are able to modify the node tree, each modification will trigger a notification for all views. Then, each view can react differently to the modification and maintain a separate internal state.
 
