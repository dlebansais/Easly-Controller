# Easly-Controller

Manage and edit Easly source code.

Easly source code is defined in the [Easly-Language](https://github.com/dlebansais/Easly-Language/) assembly. A controller, created and initialized with any of the Easly objects, can display the source code content in a .NET WPF application, allowing modification of the source code.

# TODO

- [ ] More layers.

# Architecture

The controller is made of several layers, each layer adding features to the other layer it's built on.

## Read-Only

This layer only provides read access to the source code, but introduces the core classes of the controller:

1. Indexes
An index is similar to a C++ iterator, but is fixed. It represents a child in a parent. There are five type of indexes:
+ Placeholder, a child node. This is the most straightforward index. This index has two specialized forms, one for block list replication patterns and another for block list source identifiers. 
+ Optional, a child node that is optionally assigned (throught the IOptionalReference<> interface).
+ List, a list of child nodes of the same type.
+ BlockList, a list of blocks of child nodes, all of the same type, with replication support (see the IBlockList<> interface). This index comes in two forms: an index for the first item in a block, and an index for all subsequent items in that block. Within the BlockList index, there are two variants:
  * One for a new block. This specifies the first item in the node list of the block
  * One for an existing block, to specify further items. 

Note that the read-only layer provides indexes that only allow reading source code. The next layer (Writeable, see below) introduces insertion indexes, describing children not yet inserted.

During examination of the node tree, indexes are created for new blocks. When this step is completed, and blocks initialized, they are replaced with indexes in existing blocks. 

2. States
These classes represent all data associated to a particular node in the source code tree. For the read-only layer, there isn't much to store, but subsequent layers add more info, like the existence of breakpoints and other temporary data. Blocks in block list also have a specific state class. And finally there are two dedicated state classes for block list replication patterns and another for block list source identifiers. 

3. Inners
They are the glue between states and indexes. There are as many types of inners as there are types of indexes, and given a state and inner one can obtain an index to the corresponding child.

4. Browse Context
Contexts (for short) contains data accumulated when the source code tree is parsed. For the read-only layer, this is simply a collection of indexes for each state and their children, but subsequent layers may contain more.

5. Views
One or more views can be attached to a controller. For the read-only layer, they don't do much, but when clients of the controller are able to modify the node tree, each modification will trigger a notification for all views. Then, each view can react differently to the modification and maintain a separate internal state.
 
## Writeable

The writeable layer, as the name suggests, adds features to modify the source code. The following operations can be performed:
. Replacing a node with another.
. Changing the assigned status of an optional node.
. Inserting a node in a collection (list or block list).
. Removing a node from a collection (list or block list).
. Moving a node up or down in a collection (list or block list). Note that, for block lists, a node can only be moved within a block.
. Performing operations specific to block lists:
  + Inserting a new block (removing a block is automatic when then last node in the block is removed).
  + Splitting and joining blocks.
  + Changing the replication type.
  + Moving blocks around in the block list.
. Expand or reduce a node (see below).

This layer also adds insertion indexes. They represent the index of a node to add/replace/insert rather than an existing child node. An insertion index is created by a client of the controller, and upon return of the requested operation (say, insert), the corresponding browsing index is provided. This returned index can be used for further operations like changing the node again. 

### Expanding nodes

When working with the source code it can be practical to hide values in the code that are just set to the default, but that means it can hard to find them when one wants to change them. Node expansion is provided for this purpose:
. An expanded node gets all optional nodes assigned, to a default value if there isn't one already.
. All lists that can be empty, such as arguments of a call to a method, have a default empty node added. With this feature, the user can move the cursor to this empty piece and start editing it.

The opposite operation, reduce, will unassign all optional nodes that only contains a default value, and clear lists that only contains a single default child.

Since multiple expand operations can be performed, it may be difficult to track them over time. A single "canonicalize" operation is therefore provided: it will look for any expanded node and will reduce it to its canonical, non-default parts.

## Frame

The Frame layer introduces a first level of display by creating a grid and assigning all individual component of the source code to a cell. There is no visual display, but with the cell system you can have line and column numbers.
Cells are per-view so it is possible to have a different display, with different line numbers for example, per view.

To create and assign cells, a controller view use a template, one per node (but specific to the view). A template in turn contains a hierarchy of frames dedicated to position components of the node, and from frames you can obtain cells. All templates are grouped in a template set, declared when the view is created, and that cannot be changed.

A typical use for template sets is for example a set that is verbose and another that is compact. The verbose template set will have more decoration elements and keywords, and therefore more cells. The compact template set could hide values that are set to the default thus having less cells than the verbose one.

### Cell views

They are grouped in the following categories:

1. Cell collections

These are cells containing other cells (obviously), for example to display a list. If a list contains 3 items and each of them takes just one cell, the cell collection will span 3 cells, aligned long a line or a column. Both cell views are available.

2. Single cell views

+ The empty cell, used for components that are not to be displayed, such is unassigned optional nodes.
+ The container cell, with an associated state view. This cell is a leaf in the cell tree, and indicates further calculation of line, column and other positions in the grid should use a new cell tree, created from the state and the associated template.
+ The block cell view, specifically containing an embedded cell view for a block of a block list.
+ A simple visible cell (decoration keyword and symbols...)
+ A simple focusable cell (decoration keywords that can have the focus, insertion points...)
+ A simple focusable cell associated to content (enums, boolean values in the source code such as abstract/not abstract...)
+ A simple focusable cell associated to text content (identifiers and other names...)
