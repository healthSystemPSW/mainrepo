import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FeedbacksManagerComponent } from './components/feedbacks-manager/feedbacks-manager.component';
import { FirstBuildingComponent } from './first-building/first-building.component';
import { HospitalOverviewComponent } from './hospital-overview/hospital-overview.component';
import { MaterialModule } from './material/material.module';
import { SecondBuildingComponent } from './second-building/second-building.component';
import { HomePageComponent } from './components/home-page/home-page.component';
import { EquipmentFormComponent } from './equipment-form/equipment-form.component';
import { RoomInventoryComponent } from './room-inventory/room-inventory.component';
import { RoomInfoComponent } from './room-info/room-info.component';
import { ComplaintsListComponent } from './complaints/complaints-list/complaints-list.component';
import { ComplaintDetailsComponent } from './complaints/complaint-details.component';
import { AddComplaintComponent } from './complaints/add-complaint/add-complaint.component';
import { PharmaciesListComponent } from './pharmacies/pharmacies-list.component';
import { RegisterPharmacyComponent } from './pharmacies/register-pharmacy/register-pharmacy.component';
import { HospitalEquipmentComponent } from './hospital-equipment/hospital-equipment.component';
import { MedicineSpecificationRequestsComponent } from './medicine-specification-requests/medicine-specification-requests.component';

const routes: Routes = [
  { path: 'overview', component: HospitalOverviewComponent },
  { path: 'feedbacks', component: FeedbacksManagerComponent },
  { path: 'firstBuilding', component: FirstBuildingComponent },
  { path: 'secondBuilding', component: SecondBuildingComponent },
  { path: 'roomInventory/:id', component: RoomInventoryComponent },
  { path: 'hospitalEquipment', component: HospitalEquipmentComponent },
  { path: 'firstBuilding/:roomName/:floor', component: FirstBuildingComponent },
  { path: 'secondBuilding/:roomName/:floor', component: SecondBuildingComponent },
  { path: 'complaints', component: ComplaintsListComponent },
  { path: 'complaints/:id', component: ComplaintDetailsComponent },
  { path: 'complaint-add', component: AddComplaintComponent },
  { path: 'pharmacy-register', component: RegisterPharmacyComponent },
  { path: 'pharmacy-list', component: PharmaciesListComponent },
  { path: 'medicine-specification-requests', component: MedicineSpecificationRequestsComponent},
  { path: 'home', component: HomePageComponent },
  { path: '', redirectTo: 'home', pathMatch: 'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes), CommonModule, MaterialModule],
  exports: [RouterModule],
})
export class AppRoutingModule { }
export const routingComponents = [HospitalOverviewComponent, FirstBuildingComponent, FeedbacksManagerComponent, SecondBuildingComponent, RoomInventoryComponent, HospitalEquipmentComponent];
