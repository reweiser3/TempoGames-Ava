# Requirements:

- Create a (simplified) dashboard that a player would use to view their account and focus on the ability to see stats on their gameplay. (Not authentication.)
- Make no assumptions about the length, or type of content used but feel free to be creative. Do not make use of any 3rd party services or libraries. Code should be readable, clean, and commented on.
- Feel free to make fake/pseudo models for any objects. We expect this to be done entirely inside a Blazor application.
- The focus is on user management, not specifically game related.
- Tailwind is acceptable.
- We use Entity Framework to communicate with our DB.

# Approach:
- Uses Tailwinds css as per requirement.
- Uses Flowbite as it utilizes the tailwinds css and provides a good structure for components without me writing everything from scratch,
  which I do not have the time to accomplish for a test code session. Since it was not mentioned that I cannot use flowbite, I have used it.
  While technically a third party library, I felt it was within the confines of this code test and has no true bearing on my abilities to write
  those libraries if required.
- In order to provide a true feel for how it would be to create the user I did have to use an email service provider but not making use of a
  a Nugget package to connect, we use the standard SMTP sender with SendGrid. APIKey provided in demo code. This could be swapped out for any
  SMTP provider that your desktop can connect to that is not prohibited by firewall.
- The database is initialized on first run. An Admin and Player role are created. Data populated is all fake:
  The default admin account is below and is in the role admin. The account below is already in the admin role.
  You will get a different side navigation bar based on your role.
  username: demo@tempogames.com, password: yourStrong@Password1
- Fake users are created on initial run, the demo account information can be found in Data/ApplicationDbInitializer.cs

# Dashboard
- The dashboard when logged in as a user will display:
	- User Stats from the initialized database for the currently logged in user
	- Blue Card = Your most common opponent
    - Red Card = Your most difficult opponent
    - Green Card = Your easiest opponent
- Your Friends Online list - This list is really just your 'Friends' list that are other application users. Clicking the star beside the 
  friend will make them a favorite and resort the list to put them on top.