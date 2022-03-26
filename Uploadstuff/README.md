## Deku
Adds Deku from My Hero Academia, a high risk survivor which can boost his stats and skills, in exchange for health regen and even health costs for his skills. 
#### Multiplayer works (hopefully). Standalone Ancient Scepter support BUT DO NOT USE UNTIL IT HAS UPDATED.
#### Message me on the Risk of Rain 2 Modding Discord if there are any issues- TeaL#5571. 
#### <img src="https://user-images.githubusercontent.com/93917577/160220529-efed5020-90ac-467e-98f2-27b5c162d744.png">
If you enjoy my work, support me on Ko-fi! https://ko-fi.com/tealpopcorn
## Latest Changelog, Next update(s)
- 3.1.1 
    - Hopefully fixed Float from causing crashes
    - Made the healing of St louis smash airforce base and St louis smash airforce 100% properly 'heal', so healing effects should synergise with it.
    - Balance Changes
         - Fixed health scaling with all barrier gaining and healing abilities (it was based off current health previously).
         - Adjusted st louis smash airforce base and st louis smash airforce base to 400% damage for both.

- Next update(s)
    -  Further skill reworks, balance changes

<img src="https://user-images.githubusercontent.com/93917577/158124577-1fcbdcfb-8697-4dba-8e53-207ad1e6d4e8.PNG">

## Known Issues
Pulling enemies with blackwhip is based on heaviest enemy so lighter enemies will be sent flying.

Pulling enemies with blackwhip combo is fixed so some enemies will be pulled more than others.

There may be crashes when using float or manchester? can't replicate it consistently.

Body effects like invisibility don't show up.



## Overview
    Deku's general game plan is that his base form is safe with range and crowd control but with low damage. 
    Then, when needed, he can use his specials (OFA 100% and 45%) to increase his damage and/or mobility. 
    When at low health he can rely on his passive increased regen in base form to heal up.
    Attackspeed and Movespeed scales fairly well with him as most skills do scale with it.
    Aiming to mitigate the health drain costs in OFA 100% mode can make him powerful.
    OFA 100% grants negative health regen but Deku's passive still works, meaning at lower hp it balances out.
    OFA 45% instead only allows direct healing effects rather than health regen but in turn doesn't drain health.
    One For All allows Deku to cycle between his percentages, upgrading his skills accordingly.
    Other specials have set abilities- OFA 100% focuses on close range but greater mobility while OFA 45% focuses on mid-range attacks.
    OFA Quirks grants Deku new functionality for all his base skills with the new Fa Jin Buff.

## Base Skills
### Passive
Deku has innate increased health regen the lower his health is. He has a double jump. He can sprint in any direction.

<table>
<thead>
  <tr>
    <th>Skill</th>
    <th>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</th>
    <th>Description</th>
    <th>Stats</th>
    <th>Fa Jin Buff</th>
  </tr>
</thead>
<tbody>
  <tr>
    <td>Airforce<br>Primary</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/144508155-96faa81f-a22b-4719-8680-e5382e0bc59d.png" width="100" height="100"></td>
    <td>Shoot a bullet dealing 2x100% damage.</td>
    <td>Proc: 0.5.</td>
    <td>Ricochets.</td>
  </tr>
  <tr>
    <td>Shoot Style<br>Kick<br>Primary</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/145331809-b57a4f5b-5f5b-43e1-b0af-175776969f05.png" alt="Image" width="100" height="100"></td>
    <td>Dash and kick, dealing 300% damage scaling based on movespeed.<br>Resets CD on hit and resetting all cooldowns on kill.</td>
    <td>Proc: 1.<br>CD: 6s.</td>
    <td>Freezes enemies. <br>Hits an additional time.</td>
  </tr>
  <tr>
    <td>Danger <br>Sense<br>Primary</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/158016012-dea38b44-78d5-407b-9218-f8c427e132a7.PNG" alt="Image" width="100" height="100"></td>
    <td>Activate Danger Sense, when timed properly, dodge and reset the CD.<br>Deal 600% damage to the attacker and stun those around you.<br>Attackspeed increases active window.<br>Total duration of 2 seconds.<br></td>
    <td>CD: 3s<br>Proc: 2.<br></td>
    <td>Freezes enemies.<br>Increases active window.</td>
  </tr>
  <tr>
    <td>Blackwhip<br>Secondary</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/144508171-f67f0493-5ed4-4125-b5e7-56b7b32dfa1a.png" width="100" height="100"></td>
    <td>Pulls and stuns enemies in front for 5x100% damage. <br>Gain barrier on hit. <br>Attackspeed increases the pull radius and barrier gain.</td>
    <td>Proc: 0.2.<br>CD: 3s.</td>
    <td>Doubles barrier gain.</td>
  </tr>
  <tr>
    <td>Manchester <br>Smash<br>Secondary</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/145331951-692a57ef-3ed0-4ea9-8742-4bfca1a196de.png" width="100" height="100"></td>
    <td>Jump in the air and slam down, dealing 300% damage and gaining barrier on hit, <br>Scales with movespeed.</td>
    <td>Proc: 1.<br>CD: 4s.</td>
    <td>Extra initial hit.<br>Doubles barrier gain. </td>
  </tr>
  <tr>
    <td>St Louis <br>Smash<br>Airforce<br>Secondary</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/144508292-001c3bab-9e43-4266-948c-3fee70b976ab.png" alt="Image" width="100" height="100"></td>
    <td>St Louis Smash, kicking multiple blasts of air pressure in front of you, dealing 400% damage.<br>Heal on hit, scaling with attackspeed.</td>
    <td>Proc: 0.2.<br>CD: 4s.<br></td>
    <td>Doubles range.<br>Doubles healing.</td>
  </tr>
  <tr>
    <td>Float<br>Utility</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/158124453-6c110889-72ca-41df-9801-50e924675ed1.PNG" width="100" height="100"></td>
    <td>Jump and float in the air, disabling gravity, changing your special to Delaware Smash 100%. <br>Press the button again to cancel Float.</td>
    <td>CD: 10s.<br></td>
    <td>Deal 400% damage <br>around you.</td>
  </tr>
  <tr>
    <td>Delaware<br>Smash<br>100%<br>Special</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/144508192-f0969ea2-2e50-4c33-93ae-1b5f27114889.png" alt="Image" width="100" height="100"></td>
    <td>Send a blast forward, stunning and dealing 600% damage to enemies in front, while sending you backwards as well.<br>Costs 10% of max Health.</td>
    <td>Proc: 2.<br>CD: 4s.</td>
    <td>Doubles distance travelled.</td>
  </tr>
  <tr>
    <td>Oklahoma<br>Smash<br>Utility</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/158016505-965f388a-5b0d-48bb-9aeb-8cdf3177ad6b.png" width="100" height="100"></td>
    <td>Hold the button to spin around, knocking back and dealing 100% damage multiple times around you.<br>3x armor while activated but 0.2x movespeed.<br></td>
    <td>Proc: 1.<br>CD: 6s.<br></td>
    <td>Doubles number of hits.<br>AOE is larger.<br>0.4x movespeed.</td>
  </tr>
  <tr>
    <td>Detroit<br>Smash<br>Utility</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/145331993-9097cd3e-77de-4078-873a-7dff3ea45dd2.png" alt="Image" width="100" height="100"></td>
    <td>Charge up a punch that teleports you and stuns enemies, dealing 600% damage. <br>Distance is based on movespeed and attackspeed.</td>
    <td>Proc: 2.<br>CD: 4s.</td>
    <td>Doubles everything.</td>
  </tr>
</tbody>
</table>


## Special Skills
<table>
<thead>
  <tr>
    <th>Skill</th>
    <th>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</th>
    <th>Description</th>
  </tr>
</thead>
<tbody>
  <tr>
    <td>One For All</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/144508368-f2baed32-895e-495a-88f3-9d15c0a1863e.png" alt="Image" width="100" height="100"></td>
    <td>Cycle between One For All 45% and 100%, upgrading your selected skills.<br>Boosts stats corresponding to the % of One For All. <br>This skill activates 45%.</td>
  </tr>
  <tr>
    <td>Mastered<br>One For All</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/144508393-fac634ad-5dad-4f73-9773-b0c9d24b36f6.png" alt="Image" width="100" height="100"></td>
    <td>Ancient scepter grants the same effects but also 5% lifesteal at 45% and 10% lifesteal at 100%.</td>
  </tr>
  <tr>
    <td>OFA 45%</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/145332728-e5089c43-f789-4d8b-a963-7e87e2ff1a58.png" alt="Image" width="100" height="100"></td>
    <td>Push your body to its limits, gaining unique 45% moves.<br>Boosts Attackspeed(1.25x), Damage(1.5x), Movespeed(1.25x), and Armor(2.5x) but disabling all Health Regen.</td>
  </tr>
  <tr>
    <td>Infinite 45%</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/145332711-e86e5c90-1f6c-4a66-8be4-349ce4b19f36.png" alt="Image" width="100" height="100"></td>
    <td>Ancient scepter version grants the same effects but also 5% lifesteal.</td>
  </tr>
  <tr>
    <td>OFA 100%</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/150039319-8fc24d58-cfaa-456e-9c0a-9af8accc74de.png" alt="Image" width="100" height="100"></td>
    <td>Go Beyond your limits, gaining unique 100% moves.<br>Boosts Attackspeed(1.5x), Damage(2x), Movespeed(1.5x), and Armor(5x) but causes Negative Regen. <br>Passive still works.</td>
  </tr>
  <tr>
    <td>Infinite<br>100%</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/150039332-df34212d-16ea-43d4-9c21-5a48f5601163.png" alt="Image" width="100" height="100"></td>
    <td>Ancient scepter version grants the same effects but also 10% lifesteal.</td>
  </tr>
  <tr>
    <td>OFA <br>Quirks</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/148047101-479424f8-b913-4e32-bc82-7efbcb63394e.png" alt="Image" width="100" height="100"></td>
    <td>Unlock your additional quirks. This skill grants the Fa Jin buff.<br>Moving increases the buff up to 100 stacks. Gain up to 2x damage at 50 stacks.<br>Every move consumes 50 stacks. However, if a move uses 50 stacks it acts as if it were 100% without recoil.<br>In general all moves will stun and bypass armor, have double the movement, radius and range. </td>
  </tr>
  <tr>
    <td>Mastered<br>OFA<br>Quirks</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/148047107-cfedc80a-ae75-4122-894b-4b69a76c838b.png" alt="Image" width="100" height="100"></td>
    <td>Ancient Scepter doubles Fa Jin buff gain as well as upgrading the Fa Jin primary skill.</td>
  </tr>
</tbody>
</table>

## 45% and 100% versions of the base skills

<table>
<thead>
  <tr>
    <th>Skill</th>
    <th>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</th>
    <th>Description</th>
    <th>Stats</th>
  </tr>
</thead>
<tbody>
  <tr>
    <td>Airforce <br>45%<br>Primary</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/145331910-0a4b1fb1-6223-4ea4-83f5-c84d9684e820.png" alt="Image" width="100" height="100"></td>
    <td>Shoot 4 bullets with all your fingers, dealing 125% damage each.</td>
    <td>Proc: 0.25.</td>
  </tr>
  <tr>
    <td>Airforce <br>100%<br>Primary</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/145331910-0a4b1fb1-6223-4ea4-83f5-c84d9684e820.png" alt="Image" width="100" height="100"></td>
    <td>Shoot beams with your fists, stunning and dealing 200% damage.<br>Initially having 20% attackspeed, ramping up to 200%.<br>Costs 1% of max Health.</td>
    <td>Proc: 1.</td>
  </tr>
  <tr>
    <td>Shoot Style<br>Kick 45%<br>Primary</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/145331823-67001074-4bd6-4fe2-89e7-28d519cd6928.png" alt="Image" width="100" height="100"></td>
    <td>Dash and kick, dealing 300% damage scaling based on movespeed.<br>Resets CD on hit and resetting all cooldowns on kill.</td>
    <td>Proc: 1.<br>CD: 6s.</td>
  </tr>
  <tr>
    <td>Shoot Style<br>Kick 100%<br>Primary</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/145331823-67001074-4bd6-4fe2-89e7-28d519cd6928.png" alt="Image" width="100" height="100"></td>
    <td>Dash and kick, dealing 2x100% damage scaling based on movespeed.<br> Freezes every 4th hit.<br>Resets CD on hit and resetting all cooldowns on kill.<br>Costs 1% of max Health.</td>
    <td>Proc: 1.<br>CD: 6s.</td>
  </tr>
<tr>
  <tr>
    <td>Danger <br>Sense <br>45%<br>Primary</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/158016012-dea38b44-78d5-407b-9218-f8c427e132a7.PNG" alt="Image" width="100" height="100"></td>
    <td>Activate Danger Sense, when timed properly, dodge and reset the CD<br>Deal 600% damage to the attacker and stun those around you.<br>Attackspeed increases active window.<br>Total duration of 1.5 second.<br>Costs 5% of max health.<br></td>
    <td>CD: 1s<br>Proc: 2.<br></td>
  </tr>
  <tr>
    <td>Danger <br>Sense <br>100%<br>Primary</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/158016012-dea38b44-78d5-407b-9218-f8c427e132a7.PNG" alt="Image" width="100" height="100"></td>
    <td>Activate Danger Sense, when timed properly, dodge and reset the CD<br>Deal 600% damage to the attacker and freeze those around you.<br>Attackspeed increases active window.<br>Total duration of 1 second.<br>Costs 5% of max health.<br></td>
    <td>CD: 1s<br>Proc: 2.<br></td>
  </tr>
  <tr>
    <td>Blackwhip<br>45%<br>Secondary</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/145331966-38c60bc5-872a-4a0d-a30e-7238feeec167.png" alt="Image" width="100" height="100"></td>
    <td>Blackwhip enemies, pulling them right in front of you, stunning and dealing 5x100% damage. <br>Gain barrier on hit.<br>Attackspeed increases the pull radius and barrier gain.</td>
    <td>Proc: 0.5.<br>CD: 4s.</td>
  </tr>
  <tr>
    <td>Blackwhip<br>100%<br>Secondary<br></td>
    <td><img src="https://user-images.githubusercontent.com/93917577/145331966-38c60bc5-872a-4a0d-a30e-7238feeec167.png" alt="Image" width="100" height="100"></td>
    <td>Blackwhip enemies, pulling them right in front of you, stunning and dealing 3x200% damage. <br>Gain barrier on hit.<br>Attackspeed increases the pull radius and barrier gain.</td>
    <td>Proc: 1.<br>CD: 5s.</td>
  </tr>
  <tr>
    <td>Manchester <br>Smash<br>45%<br>Secondary</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/145331951-692a57ef-3ed0-4ea9-8742-4bfca1a196de.png" alt="Image" width="100" height="100"></td>
    <td>Jump in the air and slam down, dealing 300% damage.<br>Gain barrier on hit.<br>Movespeed increases damage and barrier gain.</td>
    <td>Proc: 1.<br>CD: 4s.</td>
  </tr>
  <tr>
    <td>Manchester <br>Smash<br>100%<br>Secondary</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/145331951-692a57ef-3ed0-4ea9-8742-4bfca1a196de.png" alt="Image" width="100" height="100"></td>
    <td>Jump in the air, dealing 300% and slam down, dealing 300% damage. <br>Gain barrier on each hit.<br>Movespeed increases damage and barrier gain.<br>Costs 10% of max Health.</td>
    <td>Proc: 1.<br>CD: 5s.</td>
  </tr>
  <tr>
    <td>St Louis <br>Smash<br>Airforce<br>45%<br>Secondary</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/145332001-e18ba69f-5491-499f-838f-cc26303e1aab.PNG" alt="Image" width="100" height="100"></td>
    <td>Hit enemies in front of you, stunning and pushing them, dealing 600% damage.</td>
    <td>Proc: 1.<br>CD: 5s.</td>
    <td></td>
  </tr>
  <tr>
    <td>St Louis <br>Smash<br>Airforce<br>100%<br>Secondary</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/144508292-001c3bab-9e43-4266-948c-3fee70b976ab.png" alt="Image" width="100" height="100"></td>
    <td>St Louis Smash, kicking multiple blasts of air pressure in front of you, dealing 400% damage.<br>Heal on hit, scaling with attackspeed.</td>
    <td>Proc: 0.2.<br>CD: 6s.<br></td>
    <td></td>
  </tr>
  <tr>
    <tr>
    <td>Float 45%<br>Utility</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/158124453-6c110889-72ca-41df-9801-50e924675ed1.PNG" alt="Image" width="100" height="100"></td>
    <td>Jump and float in the air, disabling gravity, changing your special to Delaware Smash 100%. <br>Press the button again to cancel Float.</td>
    <td>CD: 10s.<br></td>
  </tr>
  <tr>
    <td>Float 100%<br>Utility</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/158124453-6c110889-72ca-41df-9801-50e924675ed1.PNG" alt="Image" width="100" height="100"></td>
    <td>Jump and float in the air, disabling gravity, changing your special to Delaware Smash 100%. <br>Deal 400% damage around you as you jump.<br>Press the button again to cancel Float.<br>Costs 10% of max health.</td>
    <td>CD: 10s.<br></td>
  </tr>
  <tr>
    <td>Oklahoma<br>Smash <br>45%<br>Utility</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/158016505-965f388a-5b0d-48bb-9aeb-8cdf3177ad6b.png" alt="Image" width="100" height="100"></td>
    <td>Hold the button to spin around, knocking back and dealing 300% damage multiple times around you.<br>3x armor while activated but 0.2x movespeed.<br></td>
    <td>Proc: 1.<br>CD: 6s.<br></td>
  </tr>
  <tr>
    <td>Oklahoma<br>Smash<br>100%<br>Utility</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/158016505-965f388a-5b0d-48bb-9aeb-8cdf3177ad6b.png" alt="Image" width="100" height="100"></td>
    <td>Hold the button to spin around, knocking back and dealing 200% damage multiple times around you.<br>3x armor while activated but 0.2x movespeed.<br>Costs 10% of max health<br></td>
    <td>Proc: 1.<br>CD: 6s.<br></td>
  </tr>
  <tr>
    <td>Detroit<br>Smash<br>45%<br>Utility</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/144508215-3ebf65c6-ef1c-43cd-b41d-0b3255842191.png" alt="Image" width="100" height="100"></td>
    <td>Charge up a punch that teleports you and stuns enemies, dealing 600%-1800% damage. <br></td>
    <td>Proc: 2.<br>CD: 4s.</td>
  </tr>
  <tr>
    <td>Detroit<br>Smash<br>100%<br>Utility</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/144508215-3ebf65c6-ef1c-43cd-b41d-0b3255842191.png" alt="Image" width="100" height="100"></td>
    <td>Charge up a punch that teleports you and stuns enemies, dealing 600% damage, charging infinitely. <br>Costs 10% of max Health.</td>
    <td>Proc: 3.<br>CD: 4s.</td>
  </tr>
</tbody>
</table>

## All specific special boosted skills

<table>
<thead>
  <tr>
    <th>OFA 45% Skills</th>
    <th>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</th>
    <th>Description</th>
    <th>Stats</th>
    <th>Fa Jin Buff</th>
  </tr>
</thead>
<tbody>
  <tr>
    <td>Airforce <br>45%<br>Primary</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/145331910-0a4b1fb1-6223-4ea4-83f5-c84d9684e820.png" alt="Image" width="100" height="100"></td>
    <td>Shoot 4 bullets with all your fingers, dealing 125% damage each.</td>
    <td>Proc: 0.25.</td>
    <td></td>
  </tr>
  <tr>
    <td>Blackwhip<br>45%<br>Secondary</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/145331966-38c60bc5-872a-4a0d-a30e-7238feeec167.png" alt="Image" width="100" height="100"></td>
    <td>Blackwhip enemies, pulling them right in front of you, stunning and dealing 5x100% damage. <br>Gain barrier on hit.<br>Attackspeed increases the pull radius and barrier gain.</td>
    <td>Proc: 0.5.<br>CD: 5s.</td>
    <td></td>
  </tr>
  <tr>
    <td>St. Louis<br>Smash<br>45%<br>Utility</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/145332001-e18ba69f-5491-499f-838f-cc26303e1aab.PNG" alt="Image" width="100" height="100"></td>
    <td>Hit enemies in front of you, stunning and pushing them, dealing 600% damage.</td>
    <td>Proc: 1.<br>CD: 5s.</td>
    <td></td>
  </tr>
  <tr>
    <td>OFA 100% Skills</td>
    <td></td>
    <td>Description</td>
    <td></td>
    <td></td>
  </tr>
  <tr>
    <td>Shoot Style<br>Full Cowling<br>100%<br>Primary</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/144508292-001c3bab-9e43-4266-948c-3fee70b976ab.png" alt="Image" width="100" height="100"></td>
    <td>Dash through enemies, hitting and stunning enemies behind, dealing 200% damage.<br>Attackspeed increases the number of attacks.<br>Costs 1% of max Health.</td>
    <td>Proc: 1.<br>CD: 0.5s.</td>
    <td></td>
  </tr>
  <tr>
    <td>Detroit <br>Smash<br>100%<br>Secondary</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/144508215-3ebf65c6-ef1c-43cd-b41d-0b3255842191.png" alt="Image" width="100" height="100"></td>
    <td>Charge up a punch that teleports you and does stuns enemies, dealing a minimum of 600% damage, charging infinitely.<br>Costs 10% of max Health.</td>
    <td>Proc: 3.<br>CD: 4s.</td>
    <td></td>
  </tr>
  <tr>
    <td>Delaware<br>Smash<br>100%<br>Utility</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/144508192-f0969ea2-2e50-4c33-93ae-1b5f27114889.png" alt="Image" width="100" height="100"></td>
    <td>Send a blast forward, stunning and dealing 600% damage to enemies in front, while sending you backwards as well.<br>Costs 10% of max Health.</td>
    <td>Proc: 2.<br>CD: 4s.</td>
    <td></td>
  </tr>
  <tr>
    <td>OfA Quirks Skills</td>
    <td></td>
    <td>Description</td>
    <td></td>
    <td></td>
  </tr>
  <tr>
    <td>Fa Jin</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/148047009-774ef354-e1ab-4f3b-8f19-6bed4a9a7297.png" alt="Image" width="100" height="100"></td>
    <td>Charge up kinetic energy, dealing 50% damage multiple times around you, granting 25 stacks of Fa Jin.<br></td>
    <td>Proc: 1.</td>
    <td>Doesn't <br>consume<br>Fa Jin.</td>
  </tr>
  <tr>
    <td>Fa Jin Mastered</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/148047023-2ff7dfdf-b710-4246-a9f1-c51f4e439569.png" alt="Image" width="100" height="100"></td>
    <td>Charge up kinetic energy, dealing 50% damage multiple times around you, granting 50 stacks of Fa Jin.<br></td>
    <td>Proc: 1.</td>
    <td>Doesn't <br>consume<br>Fa Jin.</td>
  </tr>
  <tr>
    <td>Blackwhip Combo</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/148047047-eb93efe7-078c-42be-bf80-64ffdaa03f8b.jpg" alt="Image" width="100" height="100"></td>
    <td>Hit enemies in front of you and shoot blackwhip, dealing 400% damage each.<br>Tapping the skill pulls you forward while Holding the skill pulls enemies towards you.</td>
    <td>Proc: 1.</td>
    <td>Shoot 3 times<br>Increased melee hitbox. </td>
  </tr>
  <tr>
    <td>Smokescreen</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/148047077-f947c473-01f6-4cb2-b0af-a68ef744dcfa.jpg" alt="Image" width="100" height="100"></td>
    <td>Release a smokescreen, going invisible and dealing 100% damage to enemies around you.</td>
    <td>Proc: 1.</td>
    <td>Turn nearby allies invisible as well. </td>
  </tr>
</tbody>
</table>

## Numbers
##### Armor = 15 +0.5 per level
##### Damage = 10 + 2 per level
##### Regen = 1 + 0.2 per level (note: increases the lower his health is)
##### Health = 150 + 30 per level
##### Movespeed = 7

These stats are prone to change.

## Changelog
- 3.1.0 
    - Updated readme to include St louis smash skill (oops)
    - Fixed networking for DangerSense, it now works for non-hosts too!
    - Fa Jin Aura appears properly now
    - Balance Changes
         - DangerSense CD changes, base- 3s, 45%- 2s, 100%- 1s. DangerSense total duration changes, 2s, 45%- 1.5s, 100%- 1s. These changes aim to differentiate the different versions of DangerSense and to buff it as the CD was too long before.
         - St Louis Smash secondary now heals based on a portion of your max hp, scaling with attack speed, to give it a niche of its own compared to the other secondary
- 3.0.1 
    - changed deku mod version in code

- rest of changelog on github


## Future plans
##### Better animations (I animated them myself and they are not great- TCoolDzn is helping me big thanks to him!).
##### Still more Alt skills (tried to use loader hook code for blackwhip and..yea, similarly might try to implement artificer's hover for float).
##### Code clean-up (lots of leftover code that I commented out).
##### Alt skins 


## Credits
##### Big thanks to TCoolDzn for the 3D Model, future models and animations.
##### HenryMod for the template.
##### Ganondorf for networked suck code for blackwhip.
##### Enforcer/Nemesis Enforcer mod for nemesis enforcer passive code, heatcrash and shotgun code.
##### EggSkills for the alternate artificer utility, used for detroit smash.
##### MinerUnearthed for partial utility/alt utility code for blackwhip and delaware smash.
##### Ninja for partial utility code for st louis smash.
##### Daredevil for bounce code for shoot style kick.
##### Sett for haymaker code for st louis smash 45%.
##### TTGL for crit ricochet orb code for airforce fa jin buff.