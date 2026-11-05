import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit, signal } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { Observable } from 'rxjs';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatDividerModule } from '@angular/material/divider';
import { IVehiculo } from '../../models/IVehiculo';
import { ICatMarca } from '../../models/ICatMarca';
import { IMAGENES_VEHICULOS } from '../../constants/vehiculo-imagenes';

@Component({
  selector: 'app-lista-vehiculos',
  imports: [
    CommonModule,
    RouterLink,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatChipsModule,
    MatToolbarModule,
    MatProgressSpinnerModule,
    MatDividerModule,
  ],
  templateUrl: './lista-vehiculos.html',
  styleUrl: './lista-vehiculos.scss',
})
export class ListaVehiculos implements OnInit {
  private httpClient = inject(HttpClient);
  private router = inject(Router);

  // Variable de datos de vehículos
  listaVehiculos = signal<IVehiculo[]>([]);
  marcas = signal<ICatMarca[]>([]);
  cargando = signal<boolean>(true);

  // Imágenes proporcionadas
  readonly imagenes = IMAGENES_VEHICULOS;

  private readonly urlVehiculos = '/api/vehiculos';
  private readonly urlMarcas = '/api/catalogos/marcas';

  ngOnInit(): void {
    this.cargarDatos();
  }

  cargarDatos(): void {
    this.cargando.set(true);

    // Cargar marcas para mostrar el nombre en lugar de solo el ID
    this.httpClient.get<ICatMarca[]>(this.urlMarcas).subscribe({
      next: (marcas) => this.marcas.set(marcas),
      error: (err) => console.warn('No se pudieron obtener las marcas:', err),
    });

    this.obtenerVehiculos().subscribe({
      next: (vehiculos) => {
        this.listaVehiculos.set(vehiculos);
        this.cargando.set(false);
      },
      error: (err) => {
        console.error('Error al obtener vehículos:', err);
        this.cargando.set(false);
      },
    });
  }

  // Obtener la lista de vehículos
  obtenerVehiculos(): Observable<IVehiculo[]> {
    return this.httpClient.get<IVehiculo[]>(this.urlVehiculos);
  }

  // Obtener imagen rotativa según el índice
  getImagen(index: number): string {
    return this.imagenes[index % this.imagenes.length];
  }

  // Obtener nombre de la marca por ID
  getMarcaNombre(catMarcaId: number): string {
    const encontrada = this.marcas().find((m) => m.id === catMarcaId);
    return encontrada ? encontrada.marca : `Marca #${catMarcaId}`;
  }

  // Navegar al formulario de edición
  editarVehiculo(id: number): void {
    this.router.navigate(['/editar-vehiculo', id]);
  }

  // Eliminar un vehículo
  eliminarVehiculo(id: number): void {
    if (confirm('¿Estás seguro de que deseas eliminar este vehículo?')) {
      this.httpClient.delete(`${this.urlVehiculos}/${id}`).subscribe({
        next: () => {
          this.listaVehiculos.set(this.listaVehiculos().filter((v) => v.id !== id));
        },
        error: (err) => console.error('Error al eliminar el vehículo:', err),
      });
    }
  }
}
