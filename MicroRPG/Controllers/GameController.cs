﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicroRPG.Models;
using MicroRPG.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static MicroRPG.Models.Constants;

namespace MicroRPG.Controllers
{
    public class GameController : Controller
    {
        PartyService service;
        public GameController(PartyService service)
        {
            this.service = service;
        }

        [Route("Main")]
        public IActionResult Main()
        {
            if (service.GetPartyIDs()?.Length == 0)
                service.GeneratePlayers();
            string res = HttpContext.Session.GetString(SelectedEnvironment);
            if (string.IsNullOrEmpty(res))
                HttpContext.Session.SetString(SelectedEnvironment, "Forest");

            return View(service.GetGameMainVM());
        }

        [Route("PlayerDetails/{id}")]
        public IActionResult _PlayerDetails(int id)
        {
            return PartialView(service.GetPlayerDetailsVM(id));
        }

        [Route("Creatures")]
        public IActionResult _Creatures()
        {
            return _CreaturesAlternate();
            CreaturesVM creaturesVM = WorldService.GetCreaturesVM(HttpContext.Session.GetString(SelectedEnvironment));
            return PartialView(nameof(_Creatures), creaturesVM);
        }

        [Route("CreaturesAlt")]
        public IActionResult _CreaturesAlternate()
        {
            string env = HttpContext.Session.GetString(SelectedEnvironment);
            Monster[] monsters = WorldService.GetMonsters();
            if (string.IsNullOrEmpty(env))
            {
                monsters = monsters.Where(m => m.Tags.Contains(env)).ToArray();
            }
            return PartialView(nameof(_CreaturesAlternate), monsters);
        }

        [Route("Creatures/{id}")]
        public IActionResult _CreatureDetails(string id)
        {
            return PartialView(nameof(_CreatureDetails), id);
        }

        [Route("Obstacles")]
        public IActionResult _Obstacles()
        {
            return PartialView(WorldService.GetObstacles());
        }

        [Route("Obstacles/{id}")]
        public IActionResult _ObstacleDetails(string id)
        {
            return PartialView(nameof(_ObstacleDetails), id);
        }

        [Route("Puzzles")]
        public IActionResult _Puzzles()
        {
            return PartialView(WorldService.GetPuzzles());
        }

        [Route("Puzzles/{id}")]
        public IActionResult _PuzzleDetails(string id)
        {
            return PartialView(nameof(_PuzzleDetails), id);
        }
    }
}