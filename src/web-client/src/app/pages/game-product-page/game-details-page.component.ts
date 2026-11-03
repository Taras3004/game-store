import { Component } from "@angular/core";
import { GameCommentsListComponent } from "../../shared/components/game-comments-list/game-comments-list.component";
import { GameComment } from "../../models/Comment/GameComment";
import { GameDetailsComponent } from "../../shared/components/game-details/game-details.component";
import { GameDetails } from "../../models/GameDetails";

@Component({
    selector: "app-game-details-page",
    standalone: true,
    imports: [GameCommentsListComponent, GameDetailsComponent],
    templateUrl: "game-details-page.component.html",
})
export class GameDetailsPageComponent {
    gameData: GameDetails = {
        title: "Ace Combat 7: Skies Unknown Launch Edition",
        price: 59.99,
        discountedPrice: 49.99,
        rating: 4,
        reviewsCount: 162,
        platform: "PS4",
        size: "53.76 GB",
        genres: ["Action", "Simulation", "VR"],
        description:
            "Leveraging the power of the newest generation gaming consoles, Ace Combat 7: Skies Unknown will blow players away with the evolution of its graphics engine. Beautifully rendered cloud-filled skies and highly detailed cityscapes enhance the sense of unparalleled speed and set the stage for the most engaging aerial battles in the franchise's storied 20 year history.",
    };

    totalComments = 162;
    averageRating = 4.5;

    comments: GameComment[] = [
        {
            id: 1,
            authorName: "Logan Smith",
            date: "10 min ago",
            rating: 5,
            text: "I've been playing this game for about 2 hours now and it is absolutely amazing! The graphics are stunning and the gameplay is smooth.",
            type: "Default",
            replies: [
                {
                    id: 2,
                    authorName: "Emma Watson",
                    date: "5 min ago",
                    text: "Completely agree! The level design in the second chapter is mind-blowing.",
                    type: "Quote",
                    quoteText: "The graphics are stunning and the gameplay is smooth.",
                },
            ],
        },
        {
            id: 3,
            authorName: "Michael Brown",
            date: "1 hour ago",
            rating: 4,
            text: "Great experience overall, though I encountered a few minor bugs in the VR mode.",
            type: "Default",
        },
    ];

    handleAddToCart(game: GameDetails): void {
        console.log("Added to cart:", game.title);
    }
}
