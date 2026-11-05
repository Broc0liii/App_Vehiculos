import { Component, inject, OnInit, signal } from '@angular/core';
import { ICatMarca } from '../../models/ICatMarca';
import { NuevoVehiculo } from '../crear-vehiculo/crear-vehiculo';
import { HttpClient } from '@angular/common/http';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  imports: [ReactiveFormsModule],
  selector: 'app-editar-vehiculo',
  styleUrl: './editar-vehiculo.scss',
  templateUrl: './editar-vehiculo.html',
})
export class EditarVehiculo implements OnInit {
  private form = inject(FormBuilder);
  private readonly urlCatMarcas = '/api/catalogos/marcas';
  private readonly urlVehiculos = '/api/vehiculos';

  private readonly http = inject(HttpClient);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);

  marcas = signal<ICatMarca[]>([]);
  vehiculoId = signal<number>(0);
  nuevoVehiculo = signal<NuevoVehiculo>({
    year: '',
    modelo: '',
    catMarcaId: 0,
  });

  formulario = this.form.group({
    year: [''],
    modelo: [''],
    catMarcaId: [0],
  });

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.vehiculoId.set(id);

    // Cargar marcas
    this.http.get<ICatMarca[]>(this.urlCatMarcas).subscribe({
      next: (marcas) => this.marcas.set(marcas),
      error: (error) => console.error('Error al obtener las marcas:', error),
    });

    // Cargar datos del vehiculo a editar
    this.http.get<NuevoVehiculo>(`${this.urlVehiculos}/${id}`).subscribe({
      next: (vehiculo) => {
        this.formulario.patchValue({
          year: vehiculo.year,
          modelo: vehiculo.modelo,
          catMarcaId: vehiculo.catMarcaId,
        });
      },
      error: (error) => console.error('Error al obtener el vehículo:', error),
    });
  }

  editarVehiculo() {
    const valoresFormulario = this.formulario.getRawValue();
    this.nuevoVehiculo.set({
      ...valoresFormulario,
      catMarcaId: valoresFormulario.catMarcaId ? Number(valoresFormulario.catMarcaId) : 0,
    });
    console.log('Vehículo a actualizar:', this.nuevoVehiculo());

    this.http.put(`${this.urlVehiculos}/${this.vehiculoId()}`, this.nuevoVehiculo()).subscribe({
      next: (response) => {
        console.log('Vehículo actualizado exitosamente:', response);
        this.router.navigate(['/lista-vehiculos']);
      },
      error: (error) => {
        console.error('Error al actualizar el vehículo:', error);
      },
    });
  }
}
