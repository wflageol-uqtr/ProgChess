import AuthCard from "../../components/card/AuthCard";
import type { Error } from "../../utils/type";

const ErrorPage = ({
  status = 404,
  message = "Aucune page n'a été trouvé.",
}: Error) => {
  return (
    <div className="h-screen flex items-center justify-center bg-zinc-900">
      <AuthCard>
        <div className="flex flex-col text-center">
          <h2 className="text-2xl sm:text-4xl text-green-500 font-semibold">
            {status}
          </h2>
          <p className="text-white text-lg">{message}</p>
        </div>
      </AuthCard>
    </div>
  );
};

export default ErrorPage;
