<div align="center">

<img src="https://capsule-render.vercel.app/api?type=waving&color=0:0F0C29,100:302B63&height=200&section=header&text=Driver-Perspective%20VR%20Simulator&fontSize=40&fontColor=ffffff&desc=Active%20safety%20training%20in%20a%20virtual%20city&descAlignY=72&descSize=18&animation=fadeIn" width="100%" alt="Driver-Perspective VR Simulator banner"/>

### 🥽 Put on the headset, grab the wheel — and try not to hit anyone.

![Unity](https://img.shields.io/badge/Unity%203D-000000?style=for-the-badge&logo=unity&logoColor=white)
![C#](https://img.shields.io/badge/C%23-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Meta Quest](https://img.shields.io/badge/Meta%20Quest%203-0467DF?style=for-the-badge&logo=meta&logoColor=white)
![NavMesh](https://img.shields.io/badge/AI%20NavMesh-2E7D32?style=for-the-badge)
![Mixamo](https://img.shields.io/badge/Mixamo-FF6A00?style=for-the-badge)

![Place](https://img.shields.io/badge/Deggendorf%20Institute%20of%20Technology-2024--25-302B63?style=flat-square)
![Team](https://img.shields.io/badge/team-7%20members%20%7C%20team%20lead-blueviolet?style=flat-square)
![Hardware](https://img.shields.io/badge/input-racing%20wheel%20%2B%20pedals-orange?style=flat-square)

</div>

---

## 📌 At a glance

> A **VR driver-training simulator** built in Unity at THD. The driver sits in a **Meta Quest 3** headset with a real **racing wheel and pedals**, and drives through a living city — buses, traffic and **AI pedestrians** who cross, jog and wander. Day, night, fog and rain change what the driver can see; **emergency braking (AEB) and ABS** step in; every crash is detected and the **driver's reaction time is logged**.

<div align="center">
  <img src="media/vr-demo.gif" width="85%" alt="Driving through the virtual city with pedestrians and traffic"/>
  <br/><sub><i>Driving through the city: buses, traffic and AI pedestrians on the footpaths</i></sub>
</div>

| 🗓️ Built | 👥 Team | 🧠 My role | 🎮 Setup |
|:---:|:---:|:---:|:---:|
| 2024 – 2025 | 7 students | **Team lead** · pedestrian & traffic system | Meta Quest 3 · HORI racing wheel & pedals |

---

## 📑 Contents

- [What the driver experiences](#-what-the-driver-experiences)
- [Levels](#-levels)
- [How it works](#-how-it-works)
- [My part: pedestrians & traffic](#-my-part-pedestrians--traffic)
- [Code](#-code)
- [Gallery](#-gallery)
- [Project paper](#-project-paper)
- [What I learned](#-what-i-learned)

---

## 🚗 What the driver experiences

<table>
<tr>
<td width="33%" align="center" valign="top">

### 🌦️ Changing conditions
**Day, night, fog and rain** — each one changes visibility and how early hazards appear.

</td>
<td width="33%" align="center" valign="top">

### 🚶 A living city
AI **pedestrians** walk, jog and cross; **buses and cars** follow traffic routes.

</td>
<td width="33%" align="center" valign="top">

### 🛑 Active safety
**Automatic emergency braking** and **ABS** are simulated in the vehicle.

</td>
</tr>
<tr>
<td width="33%" align="center" valign="top">

### 💥 Crash detection
Collisions end the run and the **reaction time is logged** for review.

</td>
<td width="33%" align="center" valign="top">

### 🎮 Real controls
A **HORI racing wheel and pedals**; **IK-rigged hands** follow the wheel in VR.

</td>
<td width="33%" align="center" valign="top">

### 🧭 Guided routes
A red guide path shows where to drive in the tutorial and scenarios.

</td>
</tr>
</table>

---

## 🏁 Levels

| Level | Goal |
|:---:|---|
| **0 · Tutorial** | Learn the wheel, pedals and the guide path |
| **1 – 3 · Scenarios** | City drives with passenger pick-up and drop-off, getting harder level by level |

---

## 🔄 How it works

```mermaid
flowchart LR
    W[🎮 Racing wheel<br/>and pedals] --> C[Vehicle controller<br/>physics, AEB, ABS]
    C --> SC[Unity scene]
    E[🌦️ Weather & time of day] --> SC
    subgraph AI[AI city - my part]
        SP[AI spawner] --> PED[Pedestrian prefabs<br/>NavMesh Agent + wander script]
        WP[Waypoint scripts] --> TR[Traffic vehicles]
    end
    PED --> SC
    TR --> SC
    SC --> Q[🥽 Meta Quest 3]
    SC --> CR[💥 Crash detection] --> LOG[📈 Reaction-time log]
```

---

## 🧑‍💻 My part: pedestrians & traffic

I led the seven-person team and built the **pedestrian and traffic system** end to end:

1. **Characters** — Mixamo humanoids with walk, jog and run animations
2. **Walkable areas** — footpaths baked as the **only walkable surface** with NavMesh Surface and NavMesh Modifier, so pedestrians never step onto the road by accident
3. **AI spawner** — clones pedestrian prefabs across the city within a set radius
4. **Wandering** — every pedestrian has a **NavMesh Agent** and a wander script that keeps picking new random destinations on the footpaths
5. **Traffic** — waypoint scripts drive buses and cars along their routes

---

## 💻 Code

[`NPCWander.cs`](Unity_NavMesh_AI_Walk/Assets/Scripts/NPCWander.cs) — picks a random reachable point on the NavMesh and walks there, again and again:

```csharp
void Update() {
    timer += Time.deltaTime;
    if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance
        && timer >= wanderTimer) {
        agent.SetDestination(GetRandomNavMeshPoint(transform.position, wanderRadius));
        timer = 0f;
    }
}

Vector3 GetRandomNavMeshPoint(Vector3 center, float radius) {
    Vector3 randDir = Random.insideUnitSphere * radius + center;
    return NavMesh.SamplePosition(randDir, out NavMeshHit hit, radius, NavMesh.AllAreas)
        ? hit.position : center;   // only points on the walkable footpath
}
```

[`NavMeshVisualizer.cs`](Unity_NavMesh_AI_Walk/Assets/Scripts/NavMeshVisualizer.cs) — shows the baked NavMesh in the editor while tuning the footpaths.

<details>
<summary><b>▶ Use the wander script in your own scene</b></summary>

1. Bake a **NavMesh** — mark footpaths walkable and roads not walkable (NavMesh Modifier)
2. Add a **NavMesh Agent** and `NPCWander` to a character prefab
3. Set `wanderRadius` (how far a pedestrian roams) and `wanderTimer` (pause between targets)
4. Spawn several prefabs — each one wanders on its own

</details>

---

## 📸 Gallery

<table>
<tr>
<td width="50%"><img src="media/vr-city.jpg" alt="City drive with bus and pedestrians"/></td>
<td width="50%"><img src="media/vr-pedestrians.jpg" alt="Pedestrians on the footpath next to a bus"/></td>
</tr>
<tr>
<td align="center"><sub><b>City drive</b> — bus ahead, pedestrians crossing</sub></td>
<td align="center"><sub><b>Crowds</b> — AI pedestrians on the footpaths</sub></td>
</tr>
<tr>
<td width="50%"><img src="media/vr-rain.jpg" alt="Driving in the rain"/></td>
<td width="50%"><img src="media/vr-guide-path.jpg" alt="Red guide path with a jogger"/></td>
</tr>
<tr>
<td align="center"><sub><b>Rain</b> — reduced visibility</sub></td>
<td align="center"><sub><b>Guide path</b> — a jogger steps out ahead</sub></td>
</tr>
</table>

<div align="center">
<img src="media/vr-crash.jpg" width="60%" alt="Crash screen with restart button"/>
<br/><sub><b>Crash detected</b> — the run stops and the driver restarts the level</sub>
</div>

---

## 📄 Project paper

*Driver Perspective Active Safety System: A Multi-Level Simulation in Virtual World Using Unity for Developing Safe Driving* — course project paper (IEEE format), Deggendorf Institute of Technology, 2025.
I wrote the **Introduction** and the section on **implementing pedestrians and the traffic system**.

📕 [Read the paper (PDF)](CVAR%20Paper%2022402834%2022401580%2022401917%2022408646%2022405975%2012300527%2022203798%20(1).pdf)

---

## 🧠 What I learned

- 👥 **Leading a team of seven** — splitting a large Unity project into parts and integrating them
- 🧭 **AI navigation** — NavMesh baking, agents and spawning for believable crowds
- 🥽 **VR development** — performance, comfort and real input hardware on Meta Quest 3
- 🛑 **Safety thinking** — designing scenarios that test a driver's reactions

---

<div align="center">

### 👋 Built by **Ajay Arjun Mannolkar** and team
M.Sc. Mechatronics & Cyber-Physical Systems · THD Deggendorf

[![Portfolio](https://img.shields.io/badge/Portfolio-technolkar.github.io-302B63?style=for-the-badge&logo=githubpages&logoColor=white)](https://technolkar.github.io)
[![LinkedIn](https://img.shields.io/badge/LinkedIn-ajaymannolkar-0A66C2?style=for-the-badge&logo=linkedin&logoColor=white)](https://www.linkedin.com/in/ajaymannolkar)
[![YouTube](https://img.shields.io/badge/YouTube-TECHNOL%20KAR-FF0000?style=for-the-badge&logo=youtube&logoColor=white)](https://www.youtube.com/@TechnolkarInnovations)

⭐ *If you enjoyed the ride, give the repo a star!*

<img src="https://capsule-render.vercel.app/api?type=waving&color=0:302B63,100:0F0C29&height=100&section=footer" width="100%"/>

</div>
