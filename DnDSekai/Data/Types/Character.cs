using System;
using System.Collections.Generic;

namespace DnDSekai.Data.Types
{
    class Character
    {
        public string filePath;
        public string uniqueName;

        public string name;
        public string race;
        public string class_;

        public int level;
        public int exp;
        public int hp;
        public int mp;

        public Dictionary<string, int> inventory;
        public Dictionary<string, int> equipment;
        public Dictionary<string, int> currency;

        public Dictionary<string, int> stats;
        public Dictionary<string, int> skills;
        public Dictionary<string, int> status;

        public Dictionary<string, Dictionary<string, int>> spellEffects;
        public List<string> spells;

        public Dictionary<string, int> trueSkills;
        public Dictionary<string, int> trueStats;
        public Dictionary<string, Effect> skillEffects;
        public Dictionary<string, Effect> statusEffects;

        public Character(string filePath, string uniqueName)
        {
            this.filePath = filePath;
            this.uniqueName = uniqueName;

            name = "Name";
            race = "Race";
            class_ = "None";

            level = 1;
            exp = 0;
            hp = 0;
            mp = 0;

            inventory = new Dictionary<string, int>();
            equipment = new Dictionary<string, int>();
            currency = new Dictionary<string, int>();

            stats = new Dictionary<string, int>();
            skills = new Dictionary<string, int>();
            status = new Dictionary<string, int>();

            spellEffects = new Dictionary<string, Dictionary<string, int>>();
            spells = new List<string>();

            trueSkills = new Dictionary<string, int>();
            trueStats = new Dictionary<string, int>();
            skillEffects = new Dictionary<string, Effect>();
            statusEffects = new Dictionary<string, Effect>();

            stats["maxhp"] = 0;
            stats["maxmp"] = 0;
            stats["strength"] = 0;
            stats["agility"] = 0;
            stats["magic"] = 0;
            stats["intelligence"] = 0;
            stats["charisma"] = 0;
            stats["luck"] = 0;
        }

        public void SetName(string name)
        {
            this.name = name;
        }

        public void SetRace(string race)
        {
            this.race = race;
        }

        public void SetClass(string class_)
        {
            this.class_ = class_;
        }

        public void SetLevel(int level)
        {
            if(level >= this.level)
                this.level = level;
        }

        public void AddExp(int exp)
        {
            if (exp >= this.exp)
                this.exp = exp;
        }

        public void SetHp(int hp)
        {
            this.hp = hp;
        }

        public void AddHp(int hp)
        {
            this.hp += hp;
        }

        public void SetMp(int mp)
        {
            this.mp = mp;
        }

        public void AddMp(int mp)
        {
            this.mp += mp;
        }

        public int AddItem(string item, int amount = 1)
        {
            inventory.TryGetValue(item, out int inv);
            inventory[item] = inv + amount;
            return inventory[item];
        }

        public int RemoveItem(string item, int amount = 1)
        {
            int num = -1;
            if (inventory.TryGetValue(item, out int inv))
            {
                if (inv > amount)
                {
                    inventory[item] -= amount;
                    num = inventory[item];
                }
                else if (inv == amount)
                {
                    inventory.Remove(item);
                    num = 0;
                }
            }
            return num;
        }

        public bool Equip(string item)
        {
            if (inventory.ContainsKey(item))
            {
                RemoveItem(item);
                equipment.TryGetValue(item, out int eq);
                equipment[item] = eq + 1;
                return true;
            }
            return false;
        }

        public bool UnEquip(string item)
        {
            if (equipment.TryGetValue(item, out int eq))
            {
                if (eq > 1)
                {
                    equipment[item]--;
                    AddItem(item);
                }
                else if (eq == 1)
                {
                    equipment.Remove(item);
                }
                return true;
            }
            return false;
        }

        public int AddCurrency(string item, int amount = 1)
        {
            currency.TryGetValue(item, out int curr);
            currency[item] = curr + amount;
            return currency[item];
        }

        public int RemoveCurrency(string item, int amount = 1)
        {
            int num = -1;
            if (currency.TryGetValue(item, out int curr))
            {
                if (curr > amount)
                {
                    currency[item] -= amount;
                    num = inventory[item];
                }
                else if (curr == amount)
                {
                    currency.Remove(item);
                    num = 0;
                }
            }
            return num;
        }

        public void SetStats(int hp, int mp, int strength, int agility, int magic, int intelligence, int charisma, int luck)
        {
            this.hp = hp;
            this.mp = mp;
            stats["maxhp"] = hp;
            stats["maxmp"] = mp;
            stats["strength"] = strength;
            stats["agility"] = agility;
            stats["magic"] = magic;
            stats["intelligence"] = intelligence;
            stats["charisma"] = charisma;
            stats["luck"] = luck;
        }

        public bool SetStat(string stat, int value)
        {
            if (!stats.ContainsKey(stat))
                return false;

            stats[stat] = value;
            return true;
        }

        public void AddSkill(string skill, int level)
        {
            skills[skill] = level;
        }

        public bool RemoveSkill(string skill)
        {
            if (skills.ContainsKey(skill))
            {
                skills.Remove(skill);
                return true;
            }
            return false;
        }

        public void AddStatus(string name, int length)
        {
            if (status.TryGetValue(name, out int st)) {
                if (length > st && st > 0)
                    status[name] = length;
            }
            else
                status[name] = length;
        }

        public bool RemoveStatus(string name)
        {
            if (status.ContainsKey(name))
            {
                status.Remove(name);
                return true;
            }
            return false;
        }

        public void AddSpell(string spell)
        {
            if (!spells.Contains(spell))
                spells.Add(spell);
        }

        public bool RemoveSpell(string spell)
        {
            if (spells.Contains(spell))
            {
                spells.Remove(spell);
                return true;
            }
            return false;
        }

        public void AddSpellEffect(string type, string effect, int number = 1)
        {
            if (!spellEffects.ContainsKey(type))
                spellEffects[type] = new Dictionary<string, int>();

            spellEffects[type][effect] = number;
        }

        public bool RemoveSpellEffect(string type, string effect)
        {
            if (spellEffects.ContainsKey(type) && spellEffects[type].ContainsKey(effect))
            {
                spellEffects[type].Remove(effect);
                return true;
            }
            return false;
        }

        public bool RemoveSpellEffectType(string type)
        {
            if (spellEffects.ContainsKey(type)) { 
                spellEffects.Remove(type);
                return true;
            }
            return false;
        }

        public void LevelUp()
        {
            Random r = new Random();
            int temp;
            List<string> tempStats = new List<string>(stats.Keys);

            foreach (string s in tempStats)
            {
                temp = 0;
                temp = Races.GetRace(race).statGrowth[s] + Classes.GetClass(class_).statGrowth[s];
                temp += r.Next(-temp / 2, temp / 2);
                stats[s] += temp;
            }
            level++;
            UpdateStats();
        }

        public void NextTurn()
        {
            foreach (KeyValuePair<string, int> k in status)
            {
                if (k.Value == 1)
                {
                    status.Remove(k.Key);
                }
                else if (k.Value > 1)
                {
                    status[k.Key]--;
                }
            }
            UpdateStatusEffects();
        }

        public void Rest()
        {
            foreach (KeyValuePair<string, int> k in status)
            {
                if (k.Value >= 0)
                {
                    status.Remove(k.Key);
                }
            }

            hp = trueStats["maxhp"];
            mp = trueStats["maxmp"];
            UpdateStatusEffects();
        }

        public void Regen()
        {
            hp += trueStats["maxhp"] / 10;
            mp += trueStats["maxmp"] / 10;
            HpMpCheck();
        }

        public void HpMpCheck()
        {
            if (hp > trueStats["maxhp"])
                hp = trueStats["maxhp"];
            else if (hp < 0)
                hp = 0;

            if (mp > trueStats["maxmp"])
                mp = trueStats["maxmp"];
            else if (mp < 0)
                mp = 0;
        }

        public bool CheckExp()
        {
            if (exp >= GetNextExp(level))
                return true;
            else
                return false;
        }

        public int GetNextExp(int lvl)
        {
            int temp = (int)((100 * lvl + Math.Pow(50 * lvl, 1.3)) / 10);
            return temp * 10;
        }

        public string GetCheck(string name)
        {
            string check = "";
            Effect temp = new Effect();
            if (skillEffects.ContainsKey(name))
            {
                temp = skillEffects[name];
            }
            if (stats.ContainsKey(name.ToLower()))
            {
                temp.effects.TryGetValue("Modifier", out int tempInt);
                temp.effects["Modifier"] = tempInt + (stats[name.ToLower()] / 100);
            }
            Effect mult = new Effect();
            if (statusEffects.ContainsKey(name))
                temp.Merge(statusEffects[name]);

            foreach (KeyValuePair<string, int> k in temp.effects)
            {
                if (k.Key.EndsWith("Mult"))
                {
                    mult.effects[k.Key] = k.Value;
                    temp.effects.Remove(k.Key);
                }
            }

            foreach (KeyValuePair<string, int> k in temp.effects)
            {
                // TODO : Order effects with dice amount first, dice size second etc
                check += $"{k.Key}: {(mult.effects.ContainsKey(k.Key + "Mult") ? (k.Value * mult.effects[k.Key + "Mult"] + 100) / 100 : k.Value)}, ";
            }

            if (check.Length > 2) check = check[0..^2];

            foreach (string s in temp.special)
            {
                check += $"\n{s}";
            }
            return check;
        }

        public void UpdateSkills()
        {
            trueSkills = new Dictionary<string, int>();

            foreach (KeyValuePair<string, int> k in skills)
            {
                trueSkills[k.Key] = k.Value;
            }

            foreach (KeyValuePair<string, int> r in Races.GetRace(race).skills)
            {
                trueSkills.TryGetValue(r.Key, out int lvl);
                if (r.Value > lvl)
                {
                    trueSkills[r.Key] = r.Value;
                }
            }

            foreach (KeyValuePair<string, int> c in Classes.GetClass(_class).skills)
            {
                trueSkills.TryGetValue(c.Key, out int lvl);
                if (c.Value > lvl)
                {
                    trueSkills[c.Key] = c.Value;
                }
            }

            foreach (KeyValuePair<string, int> q in equipment)
            {
                foreach (KeyValuePair<string, int> e in Items.GetEquipment(q.Key).skills)
                {
                    trueSkills.TryGetValue(e.Key, out int lvl);
                    if (e.Value > lvl)
                    {
                        trueSkills[e.Key] = e.Value;
                    }
                }
            }
            if (weapon != "Unarmed")
            {
                foreach (KeyValuePair<string, int> w in Items.GetWeapon(weapon).skills)
                {
                    trueSkills.TryGetValue(w.Key, out int lvl);
                    if (w.Value > lvl)
                    {
                        trueSkills[w.Key] = w.Value;
                    }
                }
            }
        }

        public void UpdateSkillEffects()
        {
            UpdateSkills();
            skillEffects.Clear();
            foreach (KeyValuePair<string, int> k in trueSkills)
            {
                Skill temp = Skills.GetSkill(k.Key);
                foreach (string s in temp.GetEffectNames(level))
                {
                    if (!skillEffects.ContainsKey(s))
                        skillEffects[s] = new Effect();

                    skillEffects[s].Merge(temp.GetEffect(s, k.Value));
                }
            }

            if (weapon != "Unarmed")
            {
                foreach (KeyValuePair<string, Effect> k in Items.GetWeapon(weapon).damageTypes)
                {
                    if (!skillEffects.ContainsKey(k.Key))
                        skillEffects[k.Key] = new Effect();

                    skillEffects[k.Key].Merge(k.Value);
                }
            }

            UpdateStats();
        }

        public void UpdateStatusEffects()
        {
            statusEffects.Clear();
            foreach (KeyValuePair<string, int> s in status)
            {
                foreach (KeyValuePair<string, Effect> k in Statuses.GetStatus(s.Key).effects)
                {
                    if (!statusEffects.ContainsKey(k.Key))
                        statusEffects[k.Key] = new Effect();

                    statusEffects[k.Key].Merge(k.Value);
                }
            }
            UpdateStats();
        }

        public void UpdateStats()
        {
            foreach (KeyValuePair<string, int> k in stats)
            {
                trueStats[k.Key] = k.Value;
            }

            Dictionary<string, int> mult = new Dictionary<string, int>();

            foreach (KeyValuePair<string, int> e in equipment)
            {
                foreach (KeyValuePair<string, int> s in Items.GetItem(e.Key).stats)
                {
                    trueStats[s.Key] += s.Value * e.Value;
                }
            }

            if (skillEffects.ContainsKey("stats"))
            {
                foreach (KeyValuePair<string, int> k in skillEffects["stats"].effects)
                {
                    if (stats.ContainsKey(k.Key))
                    {
                        trueStats[k.Key] += k.Value;
                    }
                }
            }

            if (statusEffects.ContainsKey("stats"))
            {
                foreach (KeyValuePair<string, int> k in statusEffects["stats"].effects)
                {
                    if (stats.ContainsKey(k.Key))
                    {
                        trueStats[k.Key] += k.Value;
                    }
                }
            }

            if (skillEffects.ContainsKey("statsMult"))
            {
                mult = skillEffects["statsMult"].effects;
            }

            if (statusEffects.ContainsKey("statsMult"))
            {
                foreach (KeyValuePair<string, int> k in statusEffects["statsMult"].effects)
                {
                    if (stats.ContainsKey(k.Key))
                    {
                        mult.TryGetValue(k.Key, out int num);
                        mult[k.Key] = num + k.Value;
                    }
                }
            }

            foreach (KeyValuePair<string, int> k in mult)
            {
                trueStats[k.Key] = (stats[k.Key] * (k.Value + 100)) / 100;
            }

            HpMpCheck();
        }
    }
}
