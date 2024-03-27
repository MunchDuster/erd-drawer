# erd-drawer
<h2>What is an ERD?</h2>
Entity-Relationship Diagram (ERD) are used in database design to illustrate how data will be stored and accessed.

<h3>What does this do?</h3>
This is a basic app to make making and editting ERDs simple and fast. <br>
The project files are available for you to customize/improve/fix the app however you want. <br>
Note: If you want to push to add your changes to the project, I'm on the COMPX223 discord.

<h3>Why?</h3>
For the COMPX223 (Second year computer science paper at Waikato University, NZ). <br>
I tried using some other tools (ms paint, lucid chart, physical pen & paper) and wasn't satisfied with their speed or reliability.

<h3>Screenshot for demonstration purposes</h3>

<img width="684" alt="example screenshot of app open with example diagram" src="https://github.com/MunchDuster/erd-drawer/assets/72060614/85e2e531-7426-4851-a566-04ec12ae2fdb">


<h2>Todo list</h2>
There are still things I'd like to add:

- [ ] Better clicking and dragging
  - [ ] (bug to be fixed) Box dragging which ends over a displayable selects that displayble as well
  - [ ] Clicking will reguire holding shift to select many items at once
- [ ] Rename and change relationship quantities will have the current values in the prompt
- [ ] Ability to customise settings without needing to edit source code
  - [ ] Dark theme
  - [ ] Font selection
  - [ ] Spacing and sizing options
- [ ] Easter-eggs
- [X] Relationship improvements
  - [X] Relationships can have from 1 to 4 links now
  - [X] Lines from a relationship to its links calculate which side is best to come from and don't pick the same side as eachother.
- [X] Line improvements
  - [X] Ability to connect lines to object vertically (currently they only connect to the sides)
  - [X] Lines that conenct to the same object to space along the side (currently they overlap into the exact middle of the side of the object)
  - [X] Smarter auto-line connections (primarily when horizontally connecting items that overlap X coordinate

