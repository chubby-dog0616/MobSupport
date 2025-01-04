using Reactive.Bindings;
using MobSupport.Class.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MobSupport.Class
{
    class RoleRotator
    {
        public ReactivePropertySlim<Member?> Navigator { get; } = new();
        public ReactivePropertySlim<Member?> Typist { get; } = new();
        public ReactiveCollection<Member> Mobs {  get; } = [];

        public RoleRotator() { }

        public RoleRotator(ReactiveCollection<Member> members)
        {
            var count = members.Count() - 1;
            for (var i = 0; i == count;)
            {
                Member member = members.Random();
                members.Remove(member);

                if (Navigator.Value  is null)
                {
                    Navigator.Value = member;
                }
                else if (Typist.Value is null)
                {
                    Typist.Value = member;
                }
                else
                {
                    Mobs.AddOnScheduler(member);
                }
            }
        }

        public void Rotate()
        {
            //タイピストをその他のモブの最後尾に移動
            if (Typist.Value != null)
            {
                Mobs.AddOnScheduler(Typist.Value);
            }
            
            
            //ナビゲーターをタイピストに移動
            Typist.Value = Navigator.Value;

            //その他のモブの先頭をその他のモブから削除してナビゲーターに移動
            var nextNav  = Mobs?.First();
            if (nextNav != null)
            {
                Mobs?.RemoveOnScheduler(nextNav);
                Navigator.Value = nextNav;
            }
        }

        public void Shuffle()
        {
            var members = new List<Member>();
            
            if (Typist.Value != null)
            {
                members.Add(Typist.Value);
                Typist.Value = null;
            }

            if (Navigator.Value != null)
            {
                members.Add(Navigator.Value);
                Navigator.Value = null;
            }

            members.AddRange(Mobs);
            Mobs.Clear();

            members = members.OrderBy(m => Guid.NewGuid()).ToList();

            foreach (var member in members)
            {
                AddMember(member);
            }
        }

        public void AddMember(Member member)
        {
            if (Navigator.Value is null)
            {
                Navigator.Value = member;
                return;
            }

            if (Typist.Value is null)
            {
                Typist.Value = member;
                return ;
            }
            Mobs.AddOnScheduler(member);
        }

        public void RemoveMember(Member member)
        {
            if (Navigator.Value?.Id == member.Id)
            {
                Navigator.Value = null;
                return;
            }

            if (Typist.Value?.Id == member.Id)
            {
                Typist.Value = null;
                return;
            }

            var removeMember = Mobs.FirstOrDefault(member => member.Id == member.Id);
            if (removeMember != null)
            {
                Mobs.RemoveOnScheduler(removeMember);
            }
        }

        public void Clear()
        {
            Typist.Value = null;
            Navigator.Value = null;
            Mobs.Clear();
        }
    }
}
