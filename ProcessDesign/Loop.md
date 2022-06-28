# Loop
Loop node is used to iterate through arrays in process map. Most common arrays are
- Subfolders in folder
- Documents in folder
- Dynamic compex variables

Loop node has following configuration options:
- Complex variable - array for loop
- Current index - start position for looping, first position is 1 (it is not 0 based), usualy populated by variable Index, type long
- Updated index - updated position after iteration, usualy same variable as in Current index
- Row found - bool flag indicating if variable members were populated (member was found) or index is out of bounds of array
- Number of columns - specify number of columns/members to be extracted from array
- Variable members - specify variabels which should be populated by members

During execution loop checks for next item in array. If item is found Row foud is set to true and variable memberes are popuated. If item is not found Row found is set to false.

Best practise:
- Use same variable for Current an Updated index if there is not special requirement to use different variables
- Reset index value before loop so process can work in case it is restarted before loop
- Use decision node after loop to check value of Row found, it is more legible then branching rule
- Nested loops need separate Index variables but can share Row found variable
- Nested loop require reset of Index variable otherwise it would work only in first iteration of parent loop
- When removing items from looped array, always decrement index when item is removed (it would skip next item if not decremented)