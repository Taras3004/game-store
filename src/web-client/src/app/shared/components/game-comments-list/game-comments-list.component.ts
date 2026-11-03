import { Component, Input } from "@angular/core";
import { GameCommentComponent } from "../game-comment/game-comment.component";
import { GameComment } from "../../../models/Comment/GameComment";

@Component({
    selector: "game-comments-list",
    standalone: true,
    imports: [GameCommentComponent],
    templateUrl: "./game-comments-list.component.html",
})
export class GameCommentsListComponent {
    @Input({ required: true }) comments: GameComment[] = [];
}
