# DekuRor2
Deku mod for Ror2
Go beyond!

## Deku
Adds Deku from My Hero Academia, a high risk survivor which can boost his stats and skills, in exchange for health regen and even health costs for his skills. 
#### Multiplayer works (hopefully). Standalone Ancient Scepter support.
#### Message me on the Risk of Rain 2 Modding Discord if there are any issues- TeaL#5571. 
#### <a href="https://ko-fi.com/tealpopcorn"><img src="https://user-images.githubusercontent.com/93917577/160220529-efed5020-90ac-467e-98f2-27b5c162d744.png"> </a>
If you enjoy my work, support me on Ko-fi!
## Popcorn Factory
<b>Check out other mods from the Popcorn Factory team!</b>
<div>
    <a href="https://thunderstore.io/package/PopcornFactory/ShigarakiMod/">
        <img src="https://user-images.githubusercontent.com/93917577/168004591-39480a52-c7fe-4962-997f-cd9460bb4d4a.png"><br>
        <p>ShigarakiMod (My other Mod!)</p>
    </a>
</div>
<div>
    <a href="https://thunderstore.io/package/PopcornFactory/DittoMod/">
        <img src="https://user-images.githubusercontent.com/93917577/168004690-23b6d040-5f89-4b62-916b-c40d774bff02.png"><br>
        <p>DittoMod (My other Mod!)</p>
    </a>
</div>
<div>
    <a href="https://thunderstore.io/package/Ethanol10/Ganondorf_Mod/">
        <img src="https://cdn.discordapp.com/attachments/399901440023330816/960043613428011079/Ethanol10-Ganondorf_Mod-2.1.5.png.128x128_q95.png"><br>
        <p>Ganondorf Mod (Ethanol 10)</p>
    </a>
</div>
<div>
    <a href="https://thunderstore.io/package/BokChoyWithSoy/Phoenix_Wright_Mod/">
        <img src="https://cdn.discordapp.com/attachments/399901440023330816/960054458790850570/BokChoyWithSoy-Phoenix_Wright_Mod-1.6.2.png.128x128_q95.png"><br>
        <p>Phoenix Wright Mod (BokChoyWithSoy)</p>
    </a>
</div>
<div>
    <a href="https://thunderstore.io/package/PopcornFactory/Wisp_WarframeSurvivorMod/">
        <img src="https://cdn.discordapp.com/attachments/399901440023330816/960043613692239942/PopcornFactory-Wisp_WarframeSurvivorMod-1.0.2.png.128x128_q95.png"><br>
        <p>Wisp Mod (Popcorn Factory Team)</p>
    </a>
</div>

## Latest Changelog, Next update(s)

- 3.2.0
    - Fixed animation timings with attacks to properly scale with attackspeed.
    - Removed OFA 45% and 100% as they are basically irrelevant.
    - NEW ICONS! (Courtesy of Mr.Bones- in the making of RoRified Deku skin as well).

- Next update(s)
    -  Further skill reworks, balance changes

<img src="https://user-images.githubusercontent.com/93917577/168006847-fb5312bf-6ba3-486b-b9a9-44460377d5d8.png">

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
    <td><img src="https://user-images.githubusercontent.com/93917577/168067576-6fe6e0c8-46de-4a85-a189-e51055e05bd0.png" width="100" height="100"></td>
    <td>Shoot a bullet dealing 2x100% damage.</td>
    <td>Proc: 0.5.</td>
    <td>Ricochets.</td>
  </tr>
  <tr>
    <td>Shoot Style<br>Kick<br>Primary</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/168067698-223582e8-d3dc-402b-a010-ff0f65444a96.png" alt="Image" width="100" height="100"></td>
    <td>Dash and kick, dealing 300% damage scaling based on movespeed.<br>Resets CD on hit and resetting all cooldowns on kill.</td>
    <td>Proc: 1.<br>CD: 6s.</td>
    <td>Freezes enemies. <br>Hits an additional time.</td>
  </tr>
  <tr>
    <td>Danger <br>Sense<br>Primary</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/168067615-b5de153d-5262-4a82-bc2b-4f78c12500ba.png" alt="Image" width="100" height="100"></td>
    <td>Activate Danger Sense, when timed properly, dodge and reset the CD.<br>Deal 600% damage to the attacker and stun those around you.<br>Attackspeed increases active window.<br>Total duration of 2 seconds.<br></td>
    <td>CD: 3s<br>Proc: 2.<br></td>
    <td>Freezes enemies.<br>Increases active window.</td>
  </tr>
  <tr>
    <td>Blackwhip<br>Secondary</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/168067593-08e4af78-e6fc-4cc7-a9f0-223fbcf5859a.png" width="100" height="100"></td>
    <td>Pulls and stuns enemies in front for 5x100% damage. <br>Gain barrier on hit. <br>Attackspeed increases the pull radius and barrier gain.</td>
    <td>Proc: 0.2.<br>CD: 3s.</td>
    <td>Doubles barrier gain.</td>
  </tr>
  <tr>
    <td>Manchester <br>Smash<br>Secondary</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/168067679-05e57fc2-dbe1-4b54-b172-6bdd14ce1cd8.png" width="100" height="100"></td>
    <td>Jump in the air and slam down, dealing 300% damage and gaining barrier on hit, <br>Scales with movespeed.</td>
    <td>Proc: 1.<br>CD: 4s.</td>
    <td>Extra initial hit.<br>Doubles barrier gain. </td>
  </tr>
  <tr>
    <td>St Louis <br>Smash<br>Airforce<br>Secondary</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/168067708-e8d13a11-2fdf-4d1b-ac14-19840074f6f6.png" alt="Image" width="100" height="100"></td>
    <td>St Louis Smash, kicking multiple blasts of air pressure in front of you, dealing 400% damage.<br>Heal on hit, scaling with attackspeed.</td>
    <td>Proc: 0.2.<br>CD: 4s.<br></td>
    <td>Doubles range.<br>Doubles healing.</td>
  </tr>
  <tr>
    <td>Float<br>Utility</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/168067666-0088e927-cbbe-444a-9168-141d50a01bbe.png" width="100" height="100"></td>
    <td>Jump and float in the air, disabling gravity, changing your special to Delaware Smash 100%. <br>Press the button again to cancel Float.</td>
    <td>CD: 10s.<br></td>
    <td>Deal 400% damage <br>around you.</td>
  </tr>
  <tr>
    <td>Delaware<br>Smash<br>100%<br>Special</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/168067646-976990f5-9c1f-4774-9ecc-fd3ecdadf44e.png" alt="Image" width="100" height="100"></td>
    <td>Send a blast forward, stunning and dealing 600% damage to enemies in front, while sending you backwards as well.<br>Costs 10% of max Health.</td>
    <td>Proc: 2.<br>CD: 4s.</td>
    <td>Doubles distance travelled.</td>
  </tr>
  <tr>
    <td>Oklahoma<br>Smash<br>Utility</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/168067683-0647f5df-585b-437a-9a01-0970aa1a84e7.png" width="100" height="100"></td>
    <td>Hold the button to spin around, knocking back and dealing 100% damage multiple times around you.<br>3x armor while activated but 0.2x movespeed.<br></td>
    <td>Proc: 1.<br>CD: 6s.<br></td>
    <td>Doubles number of hits.<br>AOE is larger.<br>0.4x movespeed.</td>
  </tr>
  <tr>
    <td>Detroit<br>Smash<br>Utility</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/168067651-62ddec46-54d5-48c7-8e4d-6de75a324435.png" alt="Image" width="100" height="100"></td>
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
    <td><img src="https://user-images.githubusercontent.com/93917577/168067731-4d091c5f-86dd-4bed-b70d-279029c37869.png" alt="Image" width="100" height="100"></td>
    <td>Cycle between One For All 45% and 100%, upgrading your selected skills.<br>Boosts stats corresponding to the % of One For All. <br>This skill activates 45%.</td>
  </tr>
  <tr>
    <td>Mastered<br>One For All</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/168067740-bd057f96-c09f-4768-b1b9-2c8371828f8e.png" alt="Image" width="100" height="100"></td>
    <td>Ancient scepter grants the same effects but also 5% lifesteal at 45% and 10% lifesteal at 100%.</td>
  </tr>
  <tr>
    <td>OFA 45%</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/168067732-6079b49a-ae31-498c-8514-0e92a9aa87f8.png" alt="Image" width="100" height="100"></td>
    <td>Push your body to its limits, gaining unique 45% moves.<br>Boosts Attackspeed(1.25x), Damage(1.5x), Movespeed(1.25x), and Armor(2.5x) but disabling all Health Regen.</td>
  </tr>
  <tr>
    <td>Infinite 45%</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/168067744-04145ec6-f3af-42ce-a84a-9986a259f90d.png" alt="Image" width="100" height="100"></td>
    <td>Ancient scepter version grants the same effects but also 5% lifesteal.</td>
  </tr>
  <tr>
    <td>OFA 100%</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/168067737-77ed0308-ea3b-4da7-b829-260985ffa65f.png" alt="Image" width="100" height="100"></td>
    <td>Go Beyond your limits, gaining unique 100% moves.<br>Boosts Attackspeed(1.5x), Damage(2x), Movespeed(1.5x), and Armor(5x) but causes Negative Regen. <br>Passive still works.</td>
  </tr>
  <tr>
    <td>Infinite<br>100%</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/168067750-ce34c044-5612-46df-91d3-5699804f058d.png" alt="Image" width="100" height="100"></td>
    <td>Ancient scepter version grants the same effects but also 10% lifesteal.</td>
  </tr>
  <tr>
    <td>OFA <br>Quirks</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/168067696-87464032-08df-46b9-bf2f-ba3c5e521524.png" alt="Image" width="100" height="100"></td>
    <td>Unlock your additional quirks. This skill grants the Fa Jin buff.<br>Moving increases the buff up to 100 stacks. Gain up to 2x damage at 50 stacks.<br>Every move consumes 50 stacks. However, if a move uses 50 stacks it acts as if it were 100% without recoil.<br>In general all moves will stun and bypass armor, have double the movement, radius and range. </td>
  </tr>
  <tr>
    <td>Mastered<br>OFA<br>Quirks</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/168067694-0db5b103-628b-48c8-9e17-9bc3e3bcf866.png" alt="Image" width="100" height="100"></td>
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
    <td><img src="https://user-images.githubusercontent.com/93917577/168067584-80ff7703-c15b-43e0-9dd5-8d143d552631.png" alt="Image" width="100" height="100"></td>
    <td>Shoot 4 bullets with all your fingers, dealing 125% damage each.</td>
    <td>Proc: 0.25.</td>
  </tr>
  <tr>
    <td>Airforce <br>100%<br>Primary</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/168067586-8c69d624-9f96-424f-930e-91e42f7aa186.png" alt="Image" width="100" height="100"></td>
    <td>Shoot beams with your fists, stunning and dealing 200% damage.<br>Initially having 20% attackspeed, ramping up to 200%.<br>Costs 1% of max Health.</td>
    <td>Proc: 1.</td>
  </tr>
  <tr>
    <td>Shoot Style<br>Kick 45%<br>Primary</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/168067701-5ab7c613-ab8a-4d30-88af-b9de985aa989.png" alt="Image" width="100" height="100"></td>
    <td>Dash and kick, dealing 300% damage scaling based on movespeed.<br>Resets CD on hit and resetting all cooldowns on kill.</td>
    <td>Proc: 1.<br>CD: 6s.</td>
  </tr>
  <tr>
    <td>Shoot Style<br>Kick 100%<br>Primary</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/168067704-0c0efb75-1fe5-4d5d-a32c-f26ed2232fe5.png" alt="Image" width="100" height="100"></td>
    <td>Dash and kick, dealing 2x100% damage scaling based on movespeed.<br> Freezes every 4th hit.<br>Resets CD on hit and resetting all cooldowns on kill.<br>Costs 1% of max Health.</td>
    <td>Proc: 1.<br>CD: 6s.</td>
  </tr>
<tr>
  <tr>
    <td>Danger <br>Sense <br>45%<br>Primary</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/168067620-a8db0925-75c0-4b58-b836-a68400f2f27c.png" alt="Image" width="100" height="100"></td>
    <td>Activate Danger Sense, when timed properly, dodge and reset the CD<br>Deal 600% damage to the attacker and stun those around you.<br>Attackspeed increases active window.<br>Total duration of 1.5 second.<br>Costs 5% of max health.<br></td>
    <td>CD: 1s<br>Proc: 2.<br></td>
  </tr>
  <tr>
    <td>Danger <br>Sense <br>100%<br>Primary</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/168067624-64bd815a-0a0d-4bbb-bff9-5605e372f649.png" alt="Image" width="100" height="100"></td>
    <td>Activate Danger Sense, when timed properly, dodge and reset the CD<br>Deal 600% damage to the attacker and freeze those around you.<br>Attackspeed increases active window.<br>Total duration of 1 second.<br>Costs 5% of max health.<br></td>
    <td>CD: 1s<br>Proc: 2.<br></td>
  </tr>
  <tr>
    <td>Blackwhip<br>45%<br>Secondary</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/168067596-5bbdb57b-292d-459c-b589-341a2d2504e4.png" alt="Image" width="100" height="100"></td>
    <td>Blackwhip enemies, pulling them right in front of you, stunning and dealing 5x100% damage. <br>Gain barrier on hit.<br>Attackspeed increases the pull radius and barrier gain.</td>
    <td>Proc: 0.5.<br>CD: 4s.</td>
  </tr>
  <tr>
    <td>Blackwhip<br>100%<br>Secondary<br></td>
    <td><img src="https://user-images.githubusercontent.com/93917577/168067599-a3851c3a-27a9-4206-8e13-a020b24ebe86.png" alt="Image" width="100" height="100"></td>
    <td>Blackwhip enemies, pulling them right in front of you, stunning and dealing 3x200% damage. <br>Gain barrier on hit.<br>Attackspeed increases the pull radius and barrier gain.</td>
    <td>Proc: 1.<br>CD: 5s.</td>
  </tr>
  <tr>
    <td>Manchester <br>Smash<br>45%<br>Secondary</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/168067680-41ce2da7-d430-4593-bf88-a6f27f618da5.png" alt="Image" width="100" height="100"></td>
    <td>Jump in the air and slam down, dealing 300% damage.<br>Gain barrier on hit.<br>Movespeed increases damage and barrier gain.</td>
    <td>Proc: 1.<br>CD: 4s.</td>
  </tr>
  <tr>
    <td>Manchester <br>Smash<br>100%<br>Secondary</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/168067682-7c2ab2e3-ae07-4fe8-92e2-f66d208f0828.png" alt="Image" width="100" height="100"></td>
    <td>Jump in the air, dealing 300% and slam down, dealing 300% damage. <br>Gain barrier on each hit.<br>Movespeed increases damage and barrier gain.<br>Costs 10% of max Health.</td>
    <td>Proc: 1.<br>CD: 5s.</td>
  </tr>
  <tr>
    <td>St Louis <br>Smash<br>Airforce<br>45%<br>Secondary</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/168067713-9b1ef895-9beb-4625-8a18-4a05424126cc.png" alt="Image" width="100" height="100"></td>
    <td>Hit enemies in front of you, stunning and pushing them, dealing 600% damage.</td>
    <td>Proc: 1.<br>CD: 5s.</td>
    <td></td>
  </tr>
  <tr>
    <td>St Louis <br>Smash<br>Airforce<br>100%<br>Secondary</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/168067716-2c815c85-4d5e-46a8-aba2-4c1a04aa1c44.png" alt="Image" width="100" height="100"></td>
    <td>St Louis Smash, kicking multiple blasts of air pressure in front of you, dealing 400% damage.<br>Heal on hit, scaling with attackspeed.</td>
    <td>Proc: 0.2.<br>CD: 6s.<br></td>
    <td></td>
  </tr>
  <tr>
    <tr>
    <td>Float 45%<br>Utility</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/168067667-a7e733be-9a82-4baa-b273-3980921dc9bb.png" alt="Image" width="100" height="100"></td>
    <td>Jump and float in the air, disabling gravity, changing your special to Delaware Smash 100%. <br>Press the button again to cancel Float.</td>
    <td>CD: 10s.<br></td>
  </tr>
  <tr>
    <td>Float 100%<br>Utility</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/168067671-0bab5cc6-4593-4436-9f29-083957d2efca.png" alt="Image" width="100" height="100"></td>
    <td>Jump and float in the air, disabling gravity, changing your special to Delaware Smash 100%. <br>Deal 400% damage around you as you jump.<br>Press the button again to cancel Float.<br>Costs 10% of max health.</td>
    <td>CD: 10s.<br></td>
  </tr>
  <tr>
    <td>Oklahoma<br>Smash <br>45%<br>Utility</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/168067687-42ea0a61-4488-4845-94b4-cc1fa8cbf244.png" alt="Image" width="100" height="100"></td>
    <td>Hold the button to spin around, knocking back and dealing 300% damage multiple times around you.<br>3x armor while activated but 0.2x movespeed.<br></td>
    <td>Proc: 1.<br>CD: 6s.<br></td>
  </tr>
  <tr>
    <td>Oklahoma<br>Smash<br>100%<br>Utility</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/168067691-4dd74130-8244-46fe-96c6-cd3178c67c66.png" alt="Image" width="100" height="100"></td>
    <td>Hold the button to spin around, knocking back and dealing 200% damage multiple times around you.<br>3x armor while activated but 0.2x movespeed.<br>Costs 10% of max health<br></td>
    <td>Proc: 1.<br>CD: 6s.<br></td>
  </tr>
  <tr>
    <td>Detroit<br>Smash<br>45%<br>Utility</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/168067654-7c6debc3-ed7d-416f-9f5c-bcf5240358f3.png" alt="Image" width="100" height="100"></td>
    <td>Charge up a punch that teleports you and stuns enemies, dealing 600%-1800% damage. <br></td>
    <td>Proc: 2.<br>CD: 4s.</td>
  </tr>
  <tr>
    <td>Detroit<br>Smash<br>100%<br>Utility</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/168067657-1b40d327-372d-4e5a-846e-cd29ee69fd8b.png" alt="Image" width="100" height="100"></td>
    <td>Charge up a punch that teleports you and stuns enemies, dealing 600% damage, charging infinitely. <br>Costs 10% of max Health.</td>
    <td>Proc: 3.<br>CD: 4s.</td>
  </tr>
</tbody>
</table>


## All specific special boosted skills

<table>
<thead>  
  <tr>
    <td>OfA Quirks Skills</td>
    <th>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</th>
    <td>Description</td>
    <td></td>
    <td></td>
  </tr>
  <tr>
    <td>Fa Jin</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/168067658-213b99f5-edac-4ce5-98d5-7cbe0067ee5d.png" alt="Image" width="100" height="100"></td>
    <td>Charge up kinetic energy, dealing 50% damage multiple times around you, granting 25 stacks of Fa Jin.<br></td>
    <td>Proc: 1.</td>
    <td>Doesn't <br>consume<br>Fa Jin.</td>
  </tr>
  <tr>
    <td>Fa Jin Mastered</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/168067663-1969cdbe-a2df-4e9e-840e-4648e0722ae0.png" alt="Image" width="100" height="100"></td>
    <td>Charge up kinetic energy, dealing 50% damage multiple times around you, granting 50 stacks of Fa Jin.<br></td>
    <td>Proc: 1.</td>
    <td>Doesn't <br>consume<br>Fa Jin.</td>
  </tr>
  <tr>
    <td>Blackwhip Combo</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/168067607-d0dac492-232e-4f43-b030-c816de5f7a93.png" alt="Image" width="100" height="100"></td>
    <td>Hit enemies in front of you and shoot blackwhip, dealing 400% damage each.<br>Tapping the skill pulls you forward while Holding the skill pulls enemies towards you.</td>
    <td>Proc: 1.</td>
    <td>Shoot 3 times<br>Increased melee hitbox. </td>
  </tr>
  <tr>
    <td>Smokescreen</td>
    <td><img src="https://user-images.githubusercontent.com/93917577/168067705-07a68f9a-3b75-4a16-9cd6-6c7c4f96ea3a.png" alt="Image" width="100" height="100"></td>
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

- 3.1.6 
    - Fixed overlays and other effects not appearing on Deku's body.
- 3.1.5 
    - Properly stated that CustomEmoteAPI is supported.
- 3.1.4 
    - EMOTES.
- 3.1.3 
    - Adjusted sound balance, fixed bug that made ditto play an unused deku voice line.

- 3.1.2 
    - Updated readme
    - Oklahoma Smash Changes: Deal a blast around yourself when you stop using the skill. Taking damage while using the skill increases your movespeed and increases the AOE and damage of the blast at the end.
- 3.1.1 
    - Hopefully fixed Float from causing crashes
    - Made the healing of St louis smash airforce base and St louis smash airforce 100% properly 'heal', so healing effects should synergise with it.
    - Balance Changes
         - Fixed health scaling with all barrier gaining and healing abilities (it was based off current health previously).
         - Adjusted st louis smash airforce base and st louis smash airforce base to 400% damage for both.

- 3.1.0 
    - Updated readme to include St louis smash skill (oops)
    - Fixed networking for DangerSense, it now works for non-hosts too!
    - Fa Jin Aura appears properly now
    - Balance Changes
         - DangerSense CD changes, base- 3s, 45%- 2s, 100%- 1s. DangerSense total duration changes, 2s, 45%- 1.5s, 100%- 1s. These changes aim to differentiate the different versions of DangerSense and to buff it as the CD was too long before.
         - St Louis Smash secondary now heals based on a portion of your max hp, scaling with attack speed, to give it a niche of its own compared to the other secondary

- rest of changelog on github

- 3.1.1 
    - Hopefully fixed Float from causing crashes
    - Made the healing of St louis smash airforce base and St louis smash airforce 100% properly 'heal', so healing effects should synergise with it.
    - Balance Changes
         - Fixed health scaling with all barrier gaining and healing abilities (it was based off current health previously).
         - Adjusted st louis smash airforce base and st louis smash airforce base to 400% damage for both.

- 3.1.0 
    - Updated readme to include St louis smash skill (oops)
    - Fixed networking for DangerSense, it now works for non-hosts too!
    - Fa Jin Aura appears properly now
    - Balance Changes
         - DangerSense CD changes, base- 3s, 45%- 2s, 100%- 1s. DangerSense total duration changes, 2s, 45%- 1.5s, 100%- 1s. These changes aim to differentiate the different versions of DangerSense and to buff it as the CD was too long before.
         - St Louis Smash secondary now heals based on a portion of your max hp, scaling with attack speed, to give it a niche of its own compared to the other secondary

- 3.0.1 
    - changed deku mod version in code
- 3.0.0

    -  Updated to patch 1.2.2
    -  Fixed logbook not showing, Fixed effects like barrier or opal having their center on his feet
    -  Added buff icons with slightly different colours than before
    -  New Model! (and some new animations!- idle, running, jumping, falling courtesy of TCoolDzn)
    -  Skill Reworks/Balance changes!
        - Adjusted barrier gain for manchester and blackwhip to % of max health instead of base damage. (This is an overall nerf)
        - Slight Fa jin rework- I've found that running around to build up the Fa jin stacks is not the best, for now:
             - Increased max buff count to 200 
             - Fa Jin primary grants 25 stacks (ancient scepter gives 50)
             - All skills grant 10 stacks
             - Stacks are only consumed when there are 50 stacks

    -  New Skills!
        - Primary skill- Danger Sense. Activate danger sense, the next attack will be dodged and the attacker will take damage. If timed properly, hit enemies around you as well.
        - Secondary skill- St louis smash airforce. Kick ranged blasts of air pressure in front of you (St louis 45% instead will be just in front of you, 100% has range again).
        - Utility Skill changes- Shoot Style (utility 1) and Shoot Style Full Cowling (utility 2)
             - Shoot Style has been changed to Float- a skill that sends you up, disabling gravity and changing your
 special to Delaware Smash. Deactivate the skill by pressing the button again. 
             - Shoot Style Full Cowling has been changed to Oklahoma Smash- while holding the skill, knockback enemies around you and have increased armor at the cost of slower movespeed.
- 2.2.0

    -  updated to void patch
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

