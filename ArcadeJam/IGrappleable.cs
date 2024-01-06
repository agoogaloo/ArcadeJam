using System;

namespace ArcadeJam;

public interface IGrappleable {
    public void GrappleStun();

    public void GrappleHit(int damage);
}
