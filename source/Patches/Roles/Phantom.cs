using TownOfUs.Extensions;
using UnityEngine;

namespace TownOfUs.Roles
{
    public class Phantom : Role
    {
        public bool Caught;
        public bool CompletedTasks;
        public bool Faded;

        public Phantom(PlayerControl player) : base(player)
        {
            Name = "Phantom";
            ImpostorText = () => "";
            TaskText = () => "Complete all your tasks without being caught!";
            Color = new Color(0.4f, 0.16f, 0.38f, 1f);
            RoleType = RoleEnum.Phantom;
            Faction = Faction.Neutral;
        }

        public void Loses()
        {
            Player.Data.SetImpostor(true);
        }

        public void Fade()
        {
            Faded = true;
            var color = new Color(1f, 1f, 1f, 0f);


            var maxDistance = ShipStatus.Instance.MaxLightRadius * PlayerControl.GameOptions.CrewLightMod;

            if (PlayerControl.LocalPlayer == null)
                return;

            var distance = (PlayerControl.LocalPlayer.GetTruePosition() - Player.GetTruePosition()).magnitude;

            var distPercent = distance / maxDistance;
            distPercent = Mathf.Max(0, distPercent - 1);

            var velocity = Player.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude;
            color.a = 0.07f + velocity / Player.MyPhysics.TrueGhostSpeed * 0.13f;
            color.a = Mathf.Lerp(color.a, 0, distPercent);

            Player.MyRend.color = color;
            //TODO: LOOK INTO THIS;
            Player.HatRenderer.SetHat("", 0);
            Player.nameText.text = "";
            if (Player.MyPhysics.Skin.skin.ProdId != DestroyableSingleton<HatManager>.Instance
                .AllSkins.ToArray()[0].ProdId)
                Player.MyPhysics.SetSkin("");
            Player.RawSetPet("", 0);

        }
    }
}