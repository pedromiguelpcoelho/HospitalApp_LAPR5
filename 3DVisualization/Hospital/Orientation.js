// ============================================================================
// Author: Team 44
// Date: 2024-11-15
// Description: This class extends the Vector2 class from three.js to represent
//              the orientation of an object in 2D space with horizontal (h) 
//              and vertical (v) components.
// Adapted from: Basic_Thumb_Raiser (SGRAI) 
// ============================================================================

import { Vector2 } from "three";

export default class Orientation extends Vector2 {
    constructor(h = 0, v = 0) {
        super();
        this.x = h;
        this.y = v;
    }

    get h() {
        return this.x;
    }

    set h(value) {
        this.x = value;
    }

    get v() {
        return this.y;
    }

    set v(value) {
        this.y = value;
    }
}