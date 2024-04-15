# erd-drawer
<h2>What is an ERD?</h2>
Entity-Relationship Diagram (ERD) are used in database design to illustrate how data will be stored and accessed.

<h3>What does this do?</h3>
This is a basic app to make making and editting ERDs simple and fast. <br>
The project files are available for you to customize/improve/fix the app however you want. <br>
Note: If you want to push to add your changes to the project, I'm on the COMPX223 discord.
You can save and load diagrams as json files as well as save images of them easily.
The S.M.A.R.T (Simple and Mighty Auto-Routing Technology) line detection system is handy too. 

<h3>Why?</h3>
For the COMPX223 (Second year computer science paper at Waikato University, NZ). <br>
I tried using some other tools (ms paint, lucid chart, physical pen & paper) and wasn't satisfied with their speed or reliability.

<h3>Screenshot of whole app window for demonstration purposes</h3>

![image](https://github.com/MunchDuster/erd-drawer/assets/72060614/1795ed83-ec7d-47a7-9e2d-607122bfc55b)


<h2>Todo list</h2>
There are still things I'd like to add:

- [ ] Ability to customise settings without needing to edit source code
  - [ ] Dark theme
  - [ ] Color options
  - [ ] font spacing options
  - [X] Font family options
  - [X] font size options
- [X] Better clicking and dragging
  - [X] (bug to be fixed) Box dragging which ends over a displayable selects that displayble as well
  - [X] Clicking will reguire holding shift to select many items at once
- [X] Rename and change relationship quantities will have the current values in the prompt
- [X] Relationship improvements
  - [X] Relationships can have from 1 to 4 links now
  - [X] Lines from a relationship to its links calculate which side is best to come from and don't pick the same side as eachother.
- [X] Line improvements
  - [X] Ability to connect lines to object vertically (currently they only connect to the sides)
  - [X] Lines that conenct to the same object to space along the side (currently they overlap into the exact middle of the side of the object)
  - [X] Smarter auto-line connections (primarily when horizontally connecting items that overlap X coordinate
