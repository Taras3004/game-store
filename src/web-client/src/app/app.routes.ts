import { Routes } from "@angular/router";
import { ContentLayoutComponent } from "./layouts/content-layout/content-layout.component";
import { MainPageComponent } from "./pages/main-page/main-page.component";
import { GameListPageComponent } from "./pages/game-list-page/game-list-page.componnent";
import { CartPageComponent } from "./pages/cart-page/cart-page.component";
import { GameDetailsPageComponent } from "./pages/game-product-page/game-details-page.component";

export const routes: Routes = [
    {
        path: "",
        component: ContentLayoutComponent,
        children: [
            { path: "", component: MainPageComponent },
            { path: "games", component: GameListPageComponent },
            { path: "game", component: GameDetailsPageComponent },
            { path: "cart", component: CartPageComponent },
        ],
    },
];
