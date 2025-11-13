## VR Walkthrough — Session Notes

- The scene now has two simple roles: `Site Engineer` and `Safety Officer`. I press `Tab` to change roles. Each role has its own marker colour.
- `Space` or `Backspace` move between three short task cards. Each card gives one question for a construction talk.
- I press `E` to drop a coloured marker where the camera looks. Every marker goes into the session log, so later I can see which places we pointed to.
- I press `R` to add a route point and `C` to clear the line. The coloured line shows which path we chose for delivery or evacuation.
- All actions are saved as JSON lines in `AppData/LocalLow/<Company>/<Product>/SessionLogs/`. Each line keeps the time, the role, the action name and sometimes the position.
- I read the log by hand. I check where our descriptions are the same and where they are different. Even a quick look in Notepad helps me catch the rhythm of the talk.
- Right now I test alone, but I plan to share my screen so two people can try the same build and create one shared log. After that I can see how the roles take turns.
- My main question: which small tool (marker, route line, task card) keeps spatial talk active during a virtual field trip? These scripts are my first small experiments with that question.
- After each run I read one short piece from `READING_NOTES.md` and then I check the log again with the new ideas in mind.
- I write quick follow-up notes in `POST_SESSION_PROMPTS.md`. If I notice the same pattern many times, I will summarise it for a small pilot report.

