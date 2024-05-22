import { Routes } from '@angular/router';
import { MainLayoutComponent } from './layouts';
import { MePageComponent } from './pages';

export const routes: Routes = [
    { 
        path: "", 
        component: MainLayoutComponent, 
        children:  
        [
            {
                path: "me",
                title: "What's for dinner | Me",
                component: MePageComponent
            }
        ] 
    }
];
