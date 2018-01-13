using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public interface ICharacterDying {
        IEnumerator onDying();
    }

    public interface ICharacterFootstep {
        void onFootstep(Const.ID.FieldType fieldtype);
    }

    public interface ICharacterWalk {
        void onForward();
        void onStay();
    }

    public interface ICharacterTargetable {
        Character target_me {
            get;
        }
    }
}