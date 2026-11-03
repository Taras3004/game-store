import { Component, EventEmitter, Input, Output } from "@angular/core";
import { GameListItemComponent } from "../game-list-item/game-list-item.component";
import { GameItem } from "../../../models/GameListItem";

@Component({
    selector: "game-list",
    standalone: true,
    imports: [GameListItemComponent],
    templateUrl: `./game-list.component.html`,
})
export class GameListComponent {
    @Input({ required: true }) games: GameItem[] = [];
    @Output() gameSelected = new EventEmitter<GameItem>();

    onGameSelect(game: GameItem): void {
        this.gameSelected.emit(game);
    }
}
