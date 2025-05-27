import { Button } from "../../components/ui/button";
import AdminLayout from "../../components/layout/AdminLayout";
import { Link } from "react-router";
import { useEffect, useState } from "react";
import api from "../../utils/api";
import { DataTable } from "../../components/table/Data-table";
import { ExerciceColumns } from "../../components/table/ExerciceColumns";
import { toast } from "sonner";

export default function ListExercice() {
  const [exercice, setExercice] = useState([]);

  const getAllExercice = async () => {
    try {
      const response = await api.get("/api/exercice");
      setExercice(response.data);
    } catch (error) {
      toast.error("Une erreur est survenue");
    }
  };

  useEffect(() => {
    getAllExercice();
  }, []);

  return (
    <AdminLayout>
      <div className="h-min-screen flex flex-col px-4 space-y-4 w-full mt-4">
        <div className="flex justify-between w-full">
          <h2 className="text-2xl font-semibold">Liste de exercice</h2>
          <Link to="/admin/exercice/create">
            <Button
              type="button"
              className="bg-green-500 hover:bg-green-700 cursor-pointer"
            >
              Ajouter
            </Button>
          </Link>
        </div>
        <DataTable columns={ExerciceColumns} data={exercice} />
      </div>
    </AdminLayout>
  );
}
