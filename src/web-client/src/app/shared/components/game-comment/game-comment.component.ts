import { Component, Input } from "@angular/core";
import { GameComment } from "../../../models/Comment/GameComment";

@Component({
    templateUrl: "game-comment.component.html",
    selector: "game-comment",
    standalone: true,
    imports: [],
})
export class GameCommentComponent {
    @Input({ required: true }) comment!: GameComment;
}
