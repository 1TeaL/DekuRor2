# DekuRor2
Deku mod for Ror2
Go beyond!

## Deku
Adds Deku from My Hero Academia, a high risk survivor which can boost his stats and skills, in exchange for health regen and even health costs for his skills. 
#### Multiplayer works (hopefully). Standalone Ancient Scepter support.
#### Message me on the Risk of Rain 2 Modding Discord if there are any issues- TeaL#5571.

<img src="https://user-images.githubusercontent.com/93917577/150066735-ab70b226-2564-4006-ae9f-3b24c1d0c70c.PNG">

## Known Issues
</p> Pulling enemies with blackwhip is based on heaviest enemy so lighter enemies will be sent flying.
<br> Pulling enemies with blackwhip combo is fixed so some enemies will be pulled more than others.

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
    <td>Shoot Style<br>Utility</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/144508290-3ac69d84-c2cf-43a4-846d-1f120d066ad5.png" alt="Image" width="100" height="100"></td>
    <td>Dashes and hits enemies for 100% damage multiple times while having invincibility during the attack (Basically mercenary's eviscerate).</td>
    <td>Proc: 0.5.<br>CD: 6s.</td>
    <td>Hold for longer duration.</td>
  </tr>
  <tr>
    <td>Shoot Style <br>Full Cowling<br>Utility</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/144509040-5999395c-19e7-40bb-9246-d2eec5d52257.png" alt="Image" width="100" height="100"></td>
    <td>Dash through enemies, hitting and stunning enemies behind, dealing 100% damage.<br>Attackspeed increases the number of attacks.</td>
    <td>Proc: 1.<br>CD: 4s.<br>Stock: 2.</td>
    <td>Doubles number of hits.</td>
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
    <td>Shoot Style<br>45%<br>Utility</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/144508290-3ac69d84-c2cf-43a4-846d-1f120d066ad5.png" alt="Image" width="100" height="100"></td>
    <td>Dashes and hits enemies for 150% damage multiple times while having invincibility during the attack.</td>
    <td>Proc: 0.5.<br>CD: 6s.</td>
  </tr>
  <tr>
    <td>Shoot Style<br>100%<br>Utility</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/144508290-3ac69d84-c2cf-43a4-846d-1f120d066ad5.png" alt="Image" width="100" height="100"></td>
    <td>Dashes and hits enemies for 100% damage multiple times while having invincibility during the attack.<br>Hold the skill to increase the duration to up to 5 seconds, but causing increased negative regen during it.<br>Costs 10% of max Health.</td>
    <td>Proc: 0.5.<br>CD: 6s.</td>
  </tr>
  <tr>
    <td>Shoot Style <br>Full Cowling<br>45%<br>Utility</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/144508292-001c3bab-9e43-4266-948c-3fee70b976ab.png" alt="Image" width="100" height="100"></td>
    <td>Dash through enemies, hitting and stunning enemies behind, dealing 150% damage.<br>Attackspeed increases the number of attacks.</td>
    <td>Proc: 1.<br>CD: 4s.<br>Stock: 2.</td>
  </tr>
  <tr>
    <td>Shoot Style <br>Full Cowling<br>100%<br>Utility</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/144508292-001c3bab-9e43-4266-948c-3fee70b976ab.png" alt="Image" width="100" height="100"></td>
    <td>Dash through enemies, hitting and stunning enemies behind, dealing 100% damage.<br>On hit, resets the CD.<br>Attackspeed increases the number of attacks.<br>Costs 5% of max Health.</td>
    <td>Proc: 1.<br>CD: 4s.<br>Stock: 2.</td>
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
    <td>Charge up kinetic energy, dealing 50% damage multiple times around you, granting 10 stacks of Fa Jin.<br></td>
    <td>Proc: 1.</td>
    <td>Doesn't <br>consume<br>Fa Jin.</td>
  </tr>
  <tr>
    <td>Fa Jin Mastered</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/148047023-2ff7dfdf-b710-4246-a9f1-c51f4e439569.png" alt="Image" width="100" height="100"></td>
    <td>Charge up kinetic energy, dealing 50% damage multiple times around you, granting 20 stacks of Fa Jin.<br></td>
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

- 2.1.0

    -  Balanced Shoot Style Kick 100%
          -  I inadvertently buffed shoot style kick last patch, it was because I forgot to actually make the damage fo the move scale by movespeed. 
          -  This resulted in the current patch move to be a lot stronger, and with 100% freezing and hitting twice (basically double damage), it was nuts. 
          -  Also, freezing constantly is nuts and makes mithrix a free win, so instead I'm gonna make it such that every 4 hits, (3 if you hit on that 4th one) it will freeze. 
          -  I've also lowered the damage from 2x300% to 2x100%- it's still strong but less so.
- 2.0.0

    -  Added another new alt special - One For All, this skill cycles Deku between OFA base, 45% and 100%. Depending on what base skills you choose, they will be upgraded accordingly. (This was my initial plan with Deku but had no idea, well now I do, and I'll still keep OFA 45% and OFA 100% as alternate skill options.)
    -  Added lightning effects to Deku's eyes when using OFA 100% to differentiate it from the 45% lightning.
    -  Updated Character select font colours.
    -  Renamed skills because of this update.
          -  The boosted 100% primary st louis smash 100% -> shoot style full cowling 100%. Should have happened a long time ago, the moves are exactly the same mechanically, and st louis smash 45% is also a different move. Although, the 100% version of shoot style full cowling through OFA Cycle will be different as the same values for a primary on a utility won't work. 
    -  Cleaned up code with skills.
          -  Fixed blackwhip 45% to properly use its numbers, it was using base blackwhips.
          -  Smokescreen now properly makes allies invisible if you're not the host.
          -  Fixed shoot style utility to use its numbers as well instead of using mercenary's eviscerate numbers.
          -  Made OFA 100% buffs take into account barrier now, so you won't die due to negative regen if you have barrier but low HP.
    -  Balanced skills.
          -  Adjusted skill cooldowns.
          -  Buffed blackwhip combo to 400% damage, added extra attacks when its Fajin Boosted too.
          -  Changed Shoot Style to now deal 100% per hit (not that it was hitting for the damage I set before), the duration has been adjust to 1 second, and the fajin buffed version has been buffed.
          -  Buffed blackwhip (and fajin buffed version) and blackwhip 45% damage, 45% also grants barrier now.
          -  The boosted 100% primary (shoot style full cowling 100%) now has deku take 1% of his health when using it, as the regen buffs are more lenient.
    -  Improved Fa Jin Buffed skills.
          -  Blackwhip and blackwhip combo buff as mentioned.
          -  Shoot style utility allows you to hold the button down to increase the duration, up to (10 seconds), now properly doubles duration and hits as well.
          -  Shoot style kick primary causes an extra AOE attack with the same damage properties, this should help with having the skill kill any frozen enemies instead of using a different skill.  
    -  Skills Added.
          -  Airforce 100%
          -  Shoot style kick 45%
          -  Shoot style kick 100%
          -  Blackwhip 100%
          -  Manchester 45%
          -  Manchester 100%
          -  Shoot Style 45%
          -  Shoot Style 100%
          -  Shoot Style Full Cowling 45%
          -  Shoot Style Full Cowling 100%
          -  Detroit Smash 45%

- 1.4.0

    -  Added another new alt special - Deku's extra quirks. This comes with new functionality for all skills.
    -  Rebalanced of OFA 100%- regen is now only -4x, and passive regen works, this means that at some point your HP will actually regen back.
    -  Nerfed Detroit Smash 100% charging damage since its easier to charge for a longer time, but the initial damage is still the same.
- 1.3.4

    -  Accidentally increased damage multiplier for OFA 100%(2 to 2.5x): was testing ways to maybe buff OFA 100% but not set yet.
    -  Lowered the volume of Deku's voice and lowered their chance of playing as well so he doesn't speak everytime.

- 1.3.3 

     - Lowered CD of manchester to 4 seconds (thought 5 was too long).
     - Networked manchester smash so no more self-damage if you were not the host. 
	- networked OFA 45% so you can get the buff (don't know how I fixed these). 
	- Fixed ragdoll by adding a bunch of exclusions to the dynamic bone script. 
	- Improved suck code for blackwhip and now works in multiplayer if you are not the host. 
                 - blackwhip and blackwhip 45% rework and rebalance- they now hit multiple times (5x).
	- Rebalanced Airforce 45% to have greater damage to 150% per bullet but harsher fall-off to further push 45%'s lower range and mobility.
    - Added Passive to be seen in the loadout now! Also made the sprinting in all directions in built into Deku passively without using a skill. 
    - ALSO new particles for airforce(s), blackwhip(s), detroit(s), delaware! LMK any thoughts about them.
- 1.3.2 
	- Properly credited model maker. 
	- Made most of his skills to make Deku enter sprinting- since most moves scale of movespeed this buffs them by default and rather than sprinting beforehand. 
	- Fixed descriptions for skills. 
	- Fixed regen code for OFA 45% so that it is always 0. 
	- Buffed OFA 100% so that at the negative regen won't kill you (also- this was previously in but the health costs of his moves could never kill Deku in the first place either) and lowered the negative regen multiplier to x-7. 
	- Buffed boosted 100% primary by removing the health cost as that was too debilitating. 
	- Halved duration of invincibility with Shoot Style Kick primary and OFA 100% St Louis primary (since it doesnt cost health). 
		- Made the damage hitbox of shoot style kick larger to fix the occasions where the hitbox didn't hurt enemies. 
		- Also adjusted the bouncing of shoot style kick, it was not consistent before. 
	- Buffed shoot style dash to have greater range. 
	- Improved code for shoot style so that it doesn't get cancelled by other attacks. 
	- Set the range of detroit smash (weak version)to be static so that it doesn't grant crazy range but now scales better with movespeed and also scales with attackspeed. 
	- Nerfed Airforce 45% to be more in line with other 45% skills (made the damage by 80% as when taken into account the damage multiplier for 45% it will be 120% per bullet, before it was 150% per bullet basically and with 5 bullets it was nuts) and also properly made it have 4 bullets instead of 5. 
	- Updated Overview page. 
	- Improved code for St Louis 45% so that it puts you in the spot when using it and decreased the duration as well. Improved the radius and made position range of Blackwhip 45% further. Improved ragdoll by having the camera follow Deku as he dies. Also forgot to update the mod version in the code.
- 1.3.1 
	- Buffed alternate primary (damage scales by movespeed, gain invincibility during use as well), fixed some naming issues and fixed secondary blackwhip not being the right skill. 
- 1.3.0 
	- Changed formula for OFA 100% special such that getting regen items won't negatively affect the skills. 
	- Added alt primary shoot style kick, alt secondary manchester smash, alt utility detroit smash, alt special OFA 45%. 
	- renamed boosted 100% skills by adding 100% to them to separate the differences between the detroit smashes. 
	- Corrected some readme errors- boosted primary invincibility duration should scale down, not remain the same. 
	- Removed walking animation and used sprinting animation for it as well- just thought it didn't look right. 
	- Some balance changes such as making the cooldown of 100% boosted primary st louis smash none again.
- 1.2.0 
	- Fixed ancient scepter support with proper 10% lifesteal. 
	- Adjusted boosted primary (added self-damage and changed the speed and duration scaling). Buffed regen passive to accommodate the higher self-damage. Added new Alt skill (Similar to boosted primary, weaker but with stun). Fixed descriptions for skills. Changed colours to descriptions. Added ragdoll. Updated Readme.
- 1.1.1 
	- fixed model issues, code clean up. (Forgot to mention previously) Changed effect of boosted primary as it may have been causing memory leaks. Changed menu colour to green. Lowered volume of voice and sfx, changed sfx of primary.
- 1.1.0 
	- added Ancient Scepter support.
- 1.0.1 
	- removed r2modman from dependencies.
- 1.0.0 
	- released

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


  
## OG Pictures
![characterselect](https://user-images.githubusercontent.com/93917577/150066735-ab70b226-2564-4006-ae9f-3b24c1d0c70c.PNG)
![airforce](https://user-images.githubusercontent.com/93917577/144508155-96faa81f-a22b-4719-8680-e5382e0bc59d.png)
![shootstylekick](https://user-images.githubusercontent.com/93917577/145331809-b57a4f5b-5f5b-43e1-b0af-175776969f05.png)
![shootstylekick2](https://user-images.githubusercontent.com/93917577/145331823-67001074-4bd6-4fe2-89e7-28d519cd6928.png)
![airforce45](https://user-images.githubusercontent.com/93917577/145331910-0a4b1fb1-6223-4ea4-83f5-c84d9684e820.png)
![stlouis](https://user-images.githubusercontent.com/93917577/144508292-001c3bab-9e43-4266-948c-3fee70b976ab.png)
![fajin](https://user-images.githubusercontent.com/93917577/148047009-774ef354-e1ab-4f3b-8f19-6bed4a9a7297.png)
![fajinboost](https://user-images.githubusercontent.com/93917577/148047023-2ff7dfdf-b710-4246-a9f1-c51f4e439569.png)


![blackwhip](https://user-images.githubusercontent.com/93917577/144508171-f67f0493-5ed4-4125-b5e7-56b7b32dfa1a.png)
![manchester](https://user-images.githubusercontent.com/93917577/145331951-692a57ef-3ed0-4ea9-8742-4bfca1a196de.png)
![detroit](https://user-images.githubusercontent.com/93917577/144508215-3ebf65c6-ef1c-43cd-b41d-0b3255842191.png) 
![blackwhipsuper](https://user-images.githubusercontent.com/93917577/145331966-38c60bc5-872a-4a0d-a30e-7238feeec167.png)
![blackwhipshoot](https://user-images.githubusercontent.com/93917577/148047047-eb93efe7-078c-42be-bf80-64ffdaa03f8b.jpg)
  
![shootstyle](https://user-images.githubusercontent.com/93917577/144508290-3ac69d84-c2cf-43a4-846d-1f120d066ad5.png)
![shootstylefullcowling](https://user-images.githubusercontent.com/93917577/144509040-5999395c-19e7-40bb-9246-d2eec5d52257.png)
![detroitweak](https://user-images.githubusercontent.com/93917577/145331993-9097cd3e-77de-4078-873a-7dff3ea45dd2.png)
![delaware](https://user-images.githubusercontent.com/93917577/144508192-f0969ea2-2e50-4c33-93ae-1b5f27114889.png)
![stlouis45](https://user-images.githubusercontent.com/93917577/145332001-e18ba69f-5491-499f-838f-cc26303e1aab.PNG)
![smokescreen](https://user-images.githubusercontent.com/93917577/148047077-f947c473-01f6-4cb2-b0af-a68ef744dcfa.jpg)

![ultimate](https://user-images.githubusercontent.com/93917577/144508368-f2baed32-895e-495a-88f3-9d15c0a1863e.png)
![ultimate45](https://user-images.githubusercontent.com/93917577/145332728-e5089c43-f789-4d8b-a963-7e87e2ff1a58.png)
![ultimateupgrade](https://user-images.githubusercontent.com/93917577/144508393-fac634ad-5dad-4f73-9773-b0c9d24b36f6.png)
![ultimateupgrade45](https://user-images.githubusercontent.com/93917577/145332711-e86e5c90-1f6c-4a66-8be4-349ce4b19f36.png)
![ultimate100](https://user-images.githubusercontent.com/93917577/150039319-8fc24d58-cfaa-456e-9c0a-9af8accc74de.png)
![ultimateupgrade100](https://user-images.githubusercontent.com/93917577/150039332-df34212d-16ea-43d4-9c21-5a48f5601163.png)
![quirksprevious](https://user-images.githubusercontent.com/93917577/148047101-479424f8-b913-4e32-bc82-7efbcb63394e.png)
![Quirks](https://user-images.githubusercontent.com/93917577/148047107-cfedc80a-ae75-4122-894b-4b69a76c838b.png)
