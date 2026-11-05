import { Routes } from '@angular/router';
import { ListaVehiculos } from './vehiculos/components/lista-vehiculos/lista-vehiculos';
import { CrearVehiculo } from './vehiculos/components/crear-vehiculo/crear-vehiculo';
import { EditarVehiculo } from './vehiculos/components/editar-vehiculo/editar-vehiculo';

export const routes: Routes = [
    {
        path: '',
        redirectTo: 'lista-vehiculos',
        pathMatch: 'full'
    },
    {
        path: 'lista-vehiculos',
        component: ListaVehiculos,
    },
    {
        path: 'crear-vehiculo',
        component: CrearVehiculo,
    },
    {
        path: 'editar-vehiculo/:id',
        component: EditarVehiculo,
    }
];
