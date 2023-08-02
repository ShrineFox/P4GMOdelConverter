using DarkUI.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace P4GMOdel
{
    public partial class MainForm : DarkForm
    {
        public static List<List<string>> animationPresets = new List<List<string>>() { //null, p4g protag, p4g party, p4g persona, p4g culprit, p3p protag, p3p party, p3p persona, p3p strega
            new List<string>{ "" },
            new List<string> { "Idle", "Idle 2", "Run", "Attack", "Attack Critical", "Placeholder 4", "Persona Change", "Persona Summon 1", "Persona Summon 2", "Persona Summon 3", "Persona Summon 4", "Guard", "Dodge", "Low HP", "Damaged", "Miss Attack", "Knocked Down", "Down", "Get Back Up", "Killed", "Revived", "Use Item", "Victory", "Pushed Out of the Way", "Placeholder 23", "Helped Up", "Placeholder 25", "Idle (Duplicate)" },
            new List<string>{ "Idle", "Idle 2", "Run", "Attack", "Attack Critical", "Special Attack 1", "Special Attack 2", "Persona Summon 1", "Persona Summon 2", "Persona Summon 3", "Persona Summon 4", "Guard", "Dodge", "Low HP", "Damaged", "Miss Attack", "Knocked Down", "Down", "Get Back Up", "Killed", "Revived", "Use Item", "Victory", "Knock Out of the Way", "Help Up Party Member", "Helped Up", "Yell At Party Member", "Idle 3", "Group Summon 1", "Group Summon 2", "Group Summon 3", "Group Summon 4" },
            new List<string>{ "Physical Attack", "Magic Attack", "Physical Attack", "Magic Attack", "Idle" },
            new List<string>{ "Idle", "Low HP", "Damaged", "Run", "Attack", "Placeholder 4", "Killed", "Miss Attack", "Knocked Down", "Down", "Get Back Up", "Persona Summon 1", "Persona Summon 2", "Persona Summon 3", "Persona Summon 4", "Idle 2", "Placeholder 15", "Low HP (Duplicate)", "Dodge" },
            new List<string>{ "Idle", "Low HP", "Damaged", "Run", "Attack", "Placeholder 4", "Dialog Animation", "Miss Attack", "Knocked Down", "Down", "Get Back Up", "Killed", "Persona Summon 1", "Persona Summon 2", "Persona Summon 3", "Persona Summon 4", "Dodge", "Idle 2" },
            new List<string>{ "Idle", "Low HP", "Damaged", "Run", "Attack", "Attack 2 Critical", "Attack 3 Critical", "Miss Attack", "Knocked Down", "Down", "Get Back Up", "Persona Summon 1", "Persona Summon 2", "Persona Summon 3", "Persona Summon 4", "Idle 2", "Use Item", "Dodge", "Revived", "Victory", "Killed", "Fusion Attack", "Guard", "Knock Out of the Way" },
            new List<string>{ "Physical Attack", "Magic Attack", "Idle", "Magic attack" },
            new List<string>{ "Idle", "Low HP", "Damaged", "Run", "Attack", "Placeholder 4", "Killed", "Miss Attack", "Knocked Down", "Down", "Get Back Up", "Persona Summon 1", "Persona Summon 2", "Persona Summon 3", "Persona Summon 4", "Idle 2", "Placeholder 15", "Low HP (Duplicate)", "Dodge" }
        };

        private void ReorderP4GProtag_Click(object sender, EventArgs e)
        {
            ReorderAnimations(1);
        }

        private void ReorderP4GParty_Click(object sender, EventArgs e)
        {
            ReorderAnimations(2);
        }

        private void ReorderP4GPersona_Click(object sender, EventArgs e)
        {
            ReorderAnimations(3);
        }

        private void ReorderP4GCulprit_Click(object sender, EventArgs e)
        {
            ReorderAnimations(4);
        }

        private void ReorderP3PProtag_Click(object sender, EventArgs e)
        {
            ReorderAnimations(5);
        }

        private void ReorderP3PParty_Click(object sender, EventArgs e)
        {
            ReorderAnimations(6);
        }

        private void ReorderP3PPersona_Click(object sender, EventArgs e)
        {
            ReorderAnimations(7);
        }

        private void ReorderP3PStrega_Click(object sender, EventArgs e)
        {
            ReorderAnimations(8);
        }

        private void ApplyP4GProtag_Click(object sender, EventArgs e)
        {
            ApplyAnimationNames(1);
        }

        private void ApplyP4GParty_Click(object sender, EventArgs e)
        {
            ApplyAnimationNames(2);
        }

        private void ApplyP4GPersona_Click(object sender, EventArgs e)
        {
            ApplyAnimationNames(3);
        }

        private void ApplyP4GCulprit_Click(object sender, EventArgs e)
        {
            ApplyAnimationNames(4);
        }

        private void ApplyP3PProtag_Click(object sender, EventArgs e)
        {
            ApplyAnimationNames(5);
        }

        private void ApplyP3PParty_Click(object sender, EventArgs e)
        {
            ApplyAnimationNames(6);
        }

        private void ApplyP3PPersona_Click(object sender, EventArgs e)
        {
            ApplyAnimationNames(7);
        }

        private void ApplyP3PStrega_Click(object sender, EventArgs e)
        {
            ApplyAnimationNames(8);
        }

        public void ApplyAnimationNames(int index)
        {
            if (model != null && model.Animations.Count > 0)
            {
                for (int i = 0; i < animationPresets[index].Count; i++)
                    if (i <= model.Animations.Count - 1)
                        model.Animations[i].Name = animationPresets[index][i];
                RefreshTreeview();
                MessageBox.Show("Applied animation names!");
            }
        }

        public void ReorderAnimations(int index)
        {
            if (model != null && model.Animations.Count > 0)
            {
                if (model.Animations.Any(x => x.Name.Contains("Magic Attack") && animationPresets[index].Any(y => y.Contains("Damaged"))))
                    MessageBox.Show("Cannot reorder Persona animations as Persona animations.");
                else if (model.Animations.Any(x => x.Name.Contains("Damaged")) && animationPresets[index].Any(y => y.Contains("Magic Attack")))
                    MessageBox.Show("Cannot reorder Persona User animations as Persona animations.");
                else if (model.Animations.Any(x => x.Name.Contains("Idle")))
                {
                    List<Animation> newAnimations = new List<Animation>();
                    for (int i = 0; i < animationPresets[index].Count; i++)
                    {
                        if (model.Animations.Any(x => x.Name.Equals(animationPresets[index][i])))
                            newAnimations.Add(model.Animations.First(x => x.Name.Equals(animationPresets[index][i])));
                        else if (model.Animations.Any(x => x.Name.StartsWith(animationPresets[index][i])))
                            newAnimations.Add(model.Animations.First(x => x.Name.StartsWith(animationPresets[index][i])));
                        else if (animationPresets[index][i].StartsWith("Placeholder"))
                            newAnimations.Add(new Animation { FrameLoop = "0.000000 0.000000", FrameRate = "30.000000", Name = $"Placeholder {i}", Animate = new List<string>(), FCurve = new List<List<string>>() });
                        else
                            newAnimations.Add(new Animation { FrameLoop = "0.000000 0.000000", FrameRate = "30.000000", Name = $"Placeholder ({animationPresets[index][i]})", Animate = new List<string>(), FCurve = new List<List<string>>() });
                    }

                    foreach (var anim in model.Animations)
                    {
                        if (anim.Animate.Count > 0 && !newAnimations.Any(x => x.Animate != null && x.Animate.Equals(anim.Animate)))
                            newAnimations.Add(anim);
                    }

                    model.Animations = newAnimations;
                    RefreshTreeview();
                    darkTreeView_Main.SelectNode(darkTreeView_Main.Nodes.First());
                    MessageBox.Show("Updated animation order!");
                }
                else
                    MessageBox.Show("Apply the animation preset that matches the current animation set first!");
            }
        }

    }
}
