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