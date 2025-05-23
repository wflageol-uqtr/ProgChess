import { useEffect } from "react";
import { useNavigate, useParams } from "react-router";
import { api } from "../utils/api";
import CookieProvider, { useCookie } from "../providers/CookieProvider";

export default function ExerciceContent() {
  const navigate = useNavigate();
  const { cookie, isLoading } = useCookie() || {};
  const { id } = useParams();

  useEffect(() => {
    if (cookie) {
      getExercice();
    } else {
      navigate("/login");
    }
  }, [cookie]);

  const getExercice = async () => {
    try {
      const response = await api.get(`/api/exercice/${id}`);
      console.log(response);
    } catch (error) {
      console.log(error);
    }
  };

  return (
    <CookieProvider>
      {isLoading ? (
        <div>
          <svg
            className="mr-3 size-5 animate-spin ..."
            viewBox="0 0 24 24"
          ></svg>
          Processing…
        </div>
      ) : (
        <div className="h-screen max-h-screen bg-zinc-900 px-4">
          <div className="h-full grid grid-flow-col grid-rows-2 gap-4">
            <div className="row-span-4 bg-yellow-400">01</div>
            <div className="col-span-2 bg-yellow-400">02</div>
            <div className="col-span-2 bg-yellow-400">03</div>
          </div>
        </div>
      )}
    </CookieProvider>
  );
}
