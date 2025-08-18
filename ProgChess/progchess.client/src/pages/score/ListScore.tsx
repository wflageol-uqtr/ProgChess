import { Link } from "react-router";
import AdminLayout from "../../components/layout/AdminLayout";
import { Button } from "../../components/ui/button";
import { useEffect, useState } from "react";
import api from "../../utils/api";
import { DataTable } from "../../components/table/Data-table";
import { ScoreColumns } from "../../components/table/ScoreColumns";
import { handleApiError } from "../../utils/apiErrorHandler";

export default function ListScore() {
  const [score, setScore] = useState([]);

  const getAllScore = async () => {
    try {
      const response = await api.get("/api/score");
      setScore(response.data);
    } catch (error) {
      handleApiError(error);
    }
  };

  useEffect(() => {
    getAllScore();
  }, []);

  return (
    <AdminLayout>
      <div className="h-min-screen flex flex-col px-4 space-y-2 md:space-y-4 w-full mt-4">
        <div className="flex justify-between w-full">
          <h2 className="text-2xl font-semibold text-white">
            Liste des résultats
          </h2>
          <Link to="/admin/score/create">
            <Button
              type="button"
              className="bg-green-500 hover:bg-green-700 cursor-pointer"
            >
              Ajouter
            </Button>
          </Link>
        </div>
        <DataTable
          columns={ScoreColumns}
          data={score}
          apiRoute="/api/score"
          filter="permanentCode"
        />
      </div>
    </AdminLayout>
  );
}
